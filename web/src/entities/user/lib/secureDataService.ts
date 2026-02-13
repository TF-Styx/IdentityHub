import { toBase64, fromBase64 } from '@/shared/lib/base64';

export class SecureDataService {
    public readonly NONCE_SIZE = 12;
    public readonly TAG_SIZE = 16;
    public readonly ITERATIONS = 600_000;
    public readonly KEY_SIZE = 32;
    private readonly MODULUS_SIZE = 384; // 3072 бит / 8

    private static readonly N = BigInt("0xB3F8C9A7D2E4F1C5A6B9D0E3F2C1A4B8D7E6F5A2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3B4C5D6E7F8A9B0C1D2E3F4A5B6C7D8E9F0A1B2C3D4E5F6A7B8C9D0E1F2A3");
    private static readonly g = BigInt(2);
    private static readonly k = BigInt(3);

    async deriveKeysFromPassword(password: string, salt: Uint8Array): Promise<{ kek: Uint8Array, authHash: string }> {
        const encoder = new TextEncoder();
        const passwordBytes = encoder.encode(password);
        const baseKey = await crypto.subtle.importKey('raw', passwordBytes, 'PBKDF2', false, ['deriveBits', 'deriveKey']);

        const masterKeyBits = await crypto.subtle.deriveBits(
            {
                name: 'PBKDF2',
                salt: salt as any,
                iterations: this.ITERATIONS,
                hash: 'SHA-256'
            },
            baseKey,
            this.KEY_SIZE * 8
        );

        const masterKey = await crypto.subtle.importKey(
            'raw',
            masterKeyBits,
            'HKDF',
            false,
            ['deriveBits']);

        const kek = await crypto.subtle.deriveBits(
            {
                name: 'HKDF',
                hash: 'SHA-256',
                salt: new Uint8Array(0),
                info: encoder.encode("AES-GCM-KEK-v1")
            },
            masterKey,
            this.KEY_SIZE * 8
        );

        const authBytes = await crypto.subtle.deriveBits(
            {
                name: 'HKDF',
                hash: 'SHA-256',
                salt: new Uint8Array(0),
                info: encoder.encode("SERVER-AUTH-HASH-v1")
            },
            masterKey,
            this.KEY_SIZE * 8
        );

        return {
            kek: new Uint8Array(kek),
            authHash: toBase64(new Uint8Array(authBytes))
        };
    }

    async encryptData<T>(dataModel: T, keyBytes: Uint8Array): Promise<string> {
        const encoder = new TextEncoder();

        let jsonString: string;

        if (dataModel instanceof Uint8Array)
            jsonString = `"${toBase64(dataModel)}"`;
         else 
            jsonString = JSON.stringify(dataModel);
        
        const plainBytes = encoder.encode(jsonString);
        const nonce = crypto.getRandomValues(new Uint8Array(this.NONCE_SIZE));

        const cryptoKey = await crypto.subtle.importKey('raw', keyBytes as any, 'AES-GCM', false, ['encrypt']);

        const encryptedContent = await crypto.subtle.encrypt(
            {
                name: 'AES-GCM',
                iv: nonce,
                tagLength: this.TAG_SIZE * 8
            },
            cryptoKey,
            plainBytes
        );

        const result = new Uint8Array(this.NONCE_SIZE + encryptedContent.byteLength);
        result.set(nonce, 0);
        result.set(new Uint8Array(encryptedContent), this.NONCE_SIZE);

        return toBase64(result);
    }

    async decryptData<T>(encryptedBase64: string, keyBytes: Uint8Array): Promise<T | null> {
        if (!encryptedBase64) return null;

        const encryptedBytes = fromBase64(encryptedBase64);

        if (encryptedBytes.length < this.NONCE_SIZE + this.TAG_SIZE) {
            throw new Error("Недопустимый формат зашифрованных данных");
        }

        const nonce = encryptedBytes.slice(0, this.NONCE_SIZE);
        const ciphertextWithTag = encryptedBytes.slice(this.NONCE_SIZE);

        const cryptoKey = await crypto.subtle.importKey(
            'raw',
            keyBytes as any,
            'AES-GCM',
            false,
            ['decrypt']
        );

        try {
            const decryptedBuffer = await crypto.subtle.decrypt(
                {
                    name: 'AES-GCM',
                    iv: nonce,
                    tagLength: this.TAG_SIZE * 8
                },
                cryptoKey,
                ciphertextWithTag
            );

            const decoder = new TextDecoder();
            const jsonString = decoder.decode(decryptedBuffer);
            return JSON.parse(jsonString) as T;
        } catch (e) {
            console.error("Ошибка дешифрования:", e);
            return null;
        }
    }

    generateRandomBytes = (length: number = 32): Uint8Array => crypto.getRandomValues(new Uint8Array(length)); 

    async generateSrpVerifier(authHash: string): Promise<string> {
        const x = this.bytesToBigInt(fromBase64(authHash));
        const v = this.expMod(SecureDataService.g, x, SecureDataService.N);

        return toBase64(this.bigIntToBytes(v, this.MODULUS_SIZE));
    }

    async generateSrpProof(password: string, saltBase64: string, B_base64: string): Promise<{ A: string, M1: string, S: string }> {
        const N = SecureDataService.N;
        const g = SecureDataService.g;
        const k = SecureDataService.k;

        const salt = fromBase64(saltBase64);
        const { authHash } = await this.deriveKeysFromPassword(password, salt);
        const x = this.bytesToBigInt(fromBase64(authHash));

        const aBytes = crypto.getRandomValues(new Uint8Array(32));
        const a = this.bytesToBigInt(aBytes);

        const A = this.expMod(g, a, N);
        if (A % N === 0n) throw new Error("Критическая ошибка: A % N === 0");

        const B_bytes = fromBase64(B_base64);
        const B = this.bytesToBigInt(B_bytes);
        if (B % N === 0n) throw new Error("Критическая ошибка: B % N === 0");

        const u = await this.hashBigInts([A, B]);
        if (u === 0n) throw new Error("Недопустимое значение u");

        const gX = this.expMod(g, x, N);
        const term = (k * gX) % N;
        const base = (B - term + N) % N;
        const exponent = a + (u * x);
        const S = this.expMod(base, exponent, N);
        if (S === 0n) throw new Error("Критическая ошибка: S === 0");

        const M1 = await this.hashBigInts([A, B, S]);

        return {
            A: toBase64(this.bigIntToBytes(A, this.MODULUS_SIZE)),
            M1: toBase64(this.bigIntToBytes(M1, 32)),
            S: toBase64(this.bigIntToBytes(S, this.MODULUS_SIZE))
        };
    }

    async verifyServerM2(A_b64: string, M1_b64: string, S_b64: string, serverM2_b64: string): Promise<boolean> {
        const A = this.bytesToBigInt(fromBase64(A_b64));
        const M1 = this.bytesToBigInt(fromBase64(M1_b64));
        const S = this.bytesToBigInt(fromBase64(S_b64));

        const computedM2 = await this.hashBigInts([A, M1, S]);
        const computedM2_b64 = toBase64(this.bigIntToBytes(computedM2, 32));

        return serverM2_b64 === computedM2_b64;
    }

    /*                          
        Публичные вспомогательные методы
    */

    // public toBase64 = (bytes: Uint8Array): string => {
    //     let binary = '';
    //     const len = bytes.byteLength;

    //     for (let i = 0; i < len; i++) 
    //         binary += String.fromCharCode(bytes[i]);
        
    //     return window.btoa(binary);
    // };

    // public fromBase64 = (base64: string): Uint8Array => {
    //     if (!base64) 
    //         throw new Error("Передана пустая строка в fromBase64");

    //     const cleaned = base64.replace(/-/g, '+').replace(/_/g, '/').replace(/\s/g, '');

    //     try {
    //         const binary_string = window.atob(cleaned);
    //         const len = binary_string.length;
    //         const bytes = new Uint8Array(len);
    //         for (let i = 0; i < len; i++) {
    //             bytes[i] = binary_string.charCodeAt(i);
    //         }
    //         return bytes;
    //     } catch (e) {
    //         console.error("Критическая ошибка Base64:", cleaned, e);
    //         throw new Error("Некорректная строка Base64.");
    //     }
    // };

    /*
        Приватные вспомогательные методы
    */

    private expMod(base: bigint, exp: bigint, mod: bigint): bigint {
        let res = BigInt(1);
        base = base % mod;

        while (exp > 0) {
            if (exp % BigInt(2) === BigInt(1)) res = (res * base) % mod;
            base = (base * base) % mod;
            exp = exp / BigInt(2);
        }
        return res;
    }

    /* Хелперы для BigInt <-> Uint8Array (Big Endian с падингом) */

    private bigIntToBytes(bn: bigint, length: number): Uint8Array {
        let hex = bn.toString(16);
        if (hex.length % 2 !== 0) hex = '0' + hex;
        const bytes = new Uint8Array(length);
        const bnBytes = this.fromHex(hex);
        const offset = Math.max(0, length - bnBytes.length);
        bytes.set(bnBytes.slice(Math.max(0, bnBytes.length - length)), offset);
        return bytes;
    }

    private bytesToBigInt(bytes: Uint8Array): bigint {
        return BigInt('0x' + Array.from(bytes).map(b => b.toString(16).padStart(2, '0')).join(''));
    }

    private fromHex(hex: string): Uint8Array {
        const matches = hex.match(/.{1,2}/g);
        return new Uint8Array(matches ? matches.map(byte => parseInt(byte, 16)) : []);
    }

    private async hashBigInts(args: bigint[]): Promise<bigint> {
        // Конвертация байт с фиксированной длиной для кроссплатформенности
        const buffers = args.map(arg => this.bigIntToBytes(arg, arg > (1n << 256n) ? this.MODULUS_SIZE : 32));
        const totalLength = buffers.reduce((acc, b) => acc + b.length, 0);
        const combined = new Uint8Array(totalLength);
        let pos = 0;
        for (const b of buffers) {
            combined.set(b, pos);
            pos += b.length;
        }

        const hashBuffer = await crypto.subtle.digest('SHA-256', combined);
        return this.bytesToBigInt(new Uint8Array(hashBuffer));
    }
}