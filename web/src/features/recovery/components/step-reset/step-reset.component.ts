import { Component, inject } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { RecoveryStateService } from "../../services/recovery-state.service";
import { SecureDataService } from "../../../../entities/user/lib/secureDataService";
import { StepResetApi } from "./step-reset.api";
import { fromBase64, toBase64 } from "../../../../shared/lib/base64";
import { RecoveryAccessPasswordRequest } from "./types";
import { firstValueFrom } from "rxjs";

@Component({selector: 'app-step-reset', templateUrl: './step-reset.component.html', styleUrls: ['./step-reset.component.scss'], standalone: true, imports: [ReactiveFormsModule]})

export class StepResetComponent{
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);
    private route = inject(ActivatedRoute);
    private state = inject(RecoveryStateService);
    private stepResetApi = inject(StepResetApi);

    stepResetForm: FormGroup;
    isLoading = false;
    errorMessage: string | null = null;

    private readonly passwordMinLength = 8;

    constructor()
    {
        this.stepResetForm = this.formBuilder.group
        (
            {
                newPassword: ['', [Validators.required, Validators.minLength(this.passwordMinLength)]],
                confirmPassword: ['', [Validators.required, Validators.minLength(this.passwordMinLength)]]
            },
            {
                validators: this.passwordMatchValidator
            }
        );
    }

    passwordMatchValidator(control: AbstractControl): ValidationErrors | null {
        const newPassword = control.get('newPassword')?.value;
        const confirmPassword = control.get('confirmPassword')?.value;

        return newPassword === confirmPassword ? null : { mismatch: true };
    }

    // onSubmit(){
    //     if (this.stepResetForm){
    //         alert('Пароль успешно изменён!');
    //         this.state.resetState();
    //         this.router.navigate(['/auth']);
    //     }
    // }
    
    secureDataService = new SecureDataService();

    async onSubmit() : Promise<void>{
        if (this.stepResetForm.invalid)
            return;

        const {newPassword: newPassword} = this.stepResetForm.value

        const publicKeyResponse = await firstValueFrom(this.stepResetApi.getPublicKey());
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
        const {kek, authHash} = await crypto.deriveKeysFromPassword(newPassword, salt);
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

        const recoveryAccessPassword: RecoveryAccessPasswordRequest = {
            Login: this.state.login!,
            Verifier: encryptedVerifierBase64,
            ClientSalt: saltBase64,
            EncryptedDek: encryptedDek,
            EncryptionAlgorithm: 'AES-GCM',
            Iterations: crypto.ITERATIONS,
            KdfType: 'PBKDF2-SHA256'
        };

        await firstValueFrom(this.stepResetApi.recoveryAccessPassword(recoveryAccessPassword));

        console.log('Пароль успешно изменён!');

        this.state.resetState();
        this.router.navigate(['/auth']);
    }
}
