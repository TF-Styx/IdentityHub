// src/features/register/model/registerStore.ts
import { create } from 'zustand';
import { SecureDataService } from '@/entities/user/lib/secureDataService';
import { registerUser } from '../api/registrationApi';
import { toBase64, fromBase64 } from '@/shared/lib/base64';
import { BFF_URL } from '@/app/config';

interface RegisterState {
  stage: 'idle' | 'encrypting' | 'sending' | 'success' | 'error';
  error: string | null;
}

// Предполагаем, что Shared.Kernel.Results.Result имеет такую структуру на клиенте
interface Result<T> {
  isSuccess: boolean;
  isFailure: boolean;
  value?: T;
  errors?: Array<{ code: string; message: string }>; // Пример структуры ошибки
}

// Обновленный интерфейс, чтобы соответствовать C# record
interface PublicKeyResponse {
  encryptionKey: string; // Изменено на 'encryptionKey'
}

export const useRegisterStore = create<RegisterState>(() => ({
  stage: 'idle',
  error: null,
}));

export const useRegisterActions = () => {
  const setState = useRegisterStore.setState;

  return {
    async register(
      login: string,
      username: string,
      email: string,
      password: string,
      phone?: string
    ) {
      setState({ stage: 'encrypting', error: null });

      try {
        // === Шаг 1: Получаем публичный ключ RSA от BFF ===
        console.log("Шаг 1");
        const bffUrl = BFF_URL;
        const pubKeyRes = await fetch(`${bffUrl}/get-public-key`, {
          credentials: 'include',
        });

        if (!pubKeyRes.ok) {
          throw new Error('Не удалось загрузить публичный ключ сервера');
        }

        // Парсим весь объект Result<PublicKeyResponse>
        const result: Result<PublicKeyResponse> = await pubKeyRes.json();

        if (result.isFailure) {
          const errorMessage = result.errors
            ? result.errors.map(e => e.message).join(', ')
            : 'Неизвестная ошибка при получении публичного ключа';
          throw new Error(errorMessage);
        }

        if (!result.value) {
            throw new Error('Ответ BFF не содержит данных публичного ключа.');
        }

        // Используйте encryptionKey вместо publicKey или PublicKey
        const publicKeyBase64 = result.value.encryptionKey;

        if (!publicKeyBase64) {
            throw new Error('Публичный ключ (EncryptionKey) не найден в ответе сервера.');
        }
        // === Шаг 2: Генерируем SRP-данные ===
        console.log("Шаг 2");
        const crypto = new SecureDataService();
        const salt = crypto.generateRandomBytes(16);
        const saltBase64 = toBase64(salt);
        const { kek, authHash } = await crypto.deriveKeysFromPassword(password, salt);
        const verifierBase64 = await crypto.generateSrpVerifier(authHash);

        // === Шаг 3: Шифруем верификатор RSA-OAEP ===

        // === Шаг 3: Шифруем верификатор RSA-OAEP ===
        console.log("Шаг 3");

        let encryptedVerifierBase64: string = '';
        
        try {
          const binaryKey = fromBase64(publicKeyBase64);
          const rsaPublicKey = await window.crypto.subtle.importKey(
            'spki',
            binaryKey.buffer as ArrayBuffer,
            { name: 'RSA-OAEP', hash: 'SHA-256' },
            false,
            ['encrypt']
          );

          const encryptedVerifierBuffer = await window.crypto.subtle.encrypt(
            { name: 'RSA-OAEP' },
            rsaPublicKey,
            fromBase64(verifierBase64).buffer as ArrayBuffer
          );
          encryptedVerifierBase64 = toBase64(new Uint8Array(encryptedVerifierBuffer));
        } catch (error) {
          console.log(error);
        }

        // === Шаг 4: Генерируем DEK и шифруем его KEK'ом ===
        console.log("Шаг 4");
        const dek = crypto.generateRandomBytes(32);
        const encryptedDek = await crypto.encryptData(dek, kek);

        // === Шаг 5: Отправляем данные на BFF ===
        console.log("Шаг 5");

        setState({ stage: 'sending', error: null });

        console.log("Дошло до регистрации");

        await registerUser({
          Login: login,
          UserName: username,
          Verifier: encryptedVerifierBase64,
          ClientSalt: saltBase64,
          EncryptedDek: encryptedDek,
          EncryptionAlgorithm: 'AES-GCM',
          Iterations: crypto.ITERATIONS,
          KdfType: 'PBKDF2-SHA256',
          Email: email,
          Phone: phone ?? null,
        });

        setState({ stage: 'success', error: null });
      } catch (err) {
        const message = err instanceof Error ? err.message : 'Неизвестная ошибка при регистрации';
        setState({ stage: 'error', error: message });
      }
    },

    reset() {
      setState({ stage: 'idle', error: null });
    },
  };
};