import { Component, inject } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { RegisterApi } from "./register.api";
import { SecureDataService } from "../../entities/user/lib/secureDataService";
import { firstValueFrom } from "rxjs";
import { fromBase64, toBase64 } from "../../shared/lib/base64";
import { RegisterRequest } from "./types";

@Component({selector: 'app-register', templateUrl: './register.component.html', styleUrls: ['./register.component.scss'], standalone: true, imports: [ReactiveFormsModule]})

export class RegisterComponent{
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);
    private registerApi = inject(RegisterApi);

    registerForm: FormGroup;
    isLoading = false;
    errorMessage: string | null = null;

    private readonly userNameMinLength = 2;
    private readonly loginMinLength = 2;
    private readonly passwordMinLength = 8;

    constructor(){
        this.registerForm = this.formBuilder.group({
            userName:   ['', [Validators.required, Validators.minLength(this.userNameMinLength)]],
            email:      ['', [Validators.required]],
            login:      ['', [Validators.required, Validators.minLength(this.loginMinLength)]],
            password:   ['', [Validators.required, Validators.minLength(this.passwordMinLength)]],
        });
    }

    secureDataService = new SecureDataService();

    async onSubmit() : Promise<void>{
        if (this.registerForm.invalid)
            return;

        const {userName: userName, email: email, login: login, password: password} = this.registerForm.value

        const publicKeyResponse = await firstValueFrom(this.registerApi.getPublicKey());
        console.log(publicKeyResponse);
        const firstParse = JSON.parse(publicKeyResponse.encryptionKey);
        const publicKeyBase64 = typeof firstParse === 'string' 
                ? firstParse 
                : firstParse.publicKey;

        if (!publicKeyBase64){
            this.errorMessage = "Ошибка получения публичного ключа!"
            return;
        }

        const crypto = new SecureDataService();
        const salt = crypto.generateRandomBytes(16);
        const saltBase64 = toBase64(salt);
        const {kek, authHash} = await crypto.deriveKeysFromPassword(password, salt);
        const verifierBase64 = await crypto.generateSrpVerifier(authHash);

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

            const a = fromBase64(verifierBase64);

            const encryptedVerifierBuffer = await window.crypto.subtle.encrypt(
                {name: 'RSA-OAEP'},
                rsaPublicKey,
                a.buffer as ArrayBuffer
            );

            encryptedVerifierBase64 = toBase64(new Uint8Array(encryptedVerifierBuffer));
        } catch (error) {
            console.log(error);
        }

        const dek = crypto.generateRandomBytes(32);
        const encryptedDek = await crypto.encryptData(dek, kek);

        console.log(`${verifierBase64}-${encryptedVerifierBase64}`);

        const registrationData: RegisterRequest = {
            Login: login,
            UserName: userName,
            Verifier: encryptedVerifierBase64,
            ClientSalt: saltBase64,
            EncryptedDek: encryptedDek,
            EncryptionAlgorithm: 'AES-GCM',
            Iterations: crypto.ITERATIONS,
            KdfType: 'PBKDF2-SHA256',
            Email: email,
            Phone: null
        };

        await firstValueFrom(this.registerApi.registration(registrationData));

        this.router.navigateByUrl("/auth");
    }
}