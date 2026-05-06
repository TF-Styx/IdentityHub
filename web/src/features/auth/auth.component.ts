import { Component, inject, ViewEncapsulation } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { Router, RouterLink } from "@angular/router";
import { AuthApi } from "./auth.api";
import { SecureDataService } from "../../entities/user/lib/secureDataService";
import { firstValueFrom } from "rxjs";

@Component({selector: 'app-auth', templateUrl: './auth.component.html', styleUrls: ['./auth.component.scss'], standalone: true, imports: [ReactiveFormsModule, RouterLink], encapsulation: ViewEncapsulation.None})

export class AuthComponent{
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);
    private authApi = inject(AuthApi);

    loginForm: FormGroup;
    isLoading = false;
    errorMessage: string | null = null;

    private readonly loginMinLength = 2;
    private readonly passwordMinLength = 8;

    constructor(){
        this.loginForm = this.formBuilder.group({
            login: ['', [Validators.required, Validators.minLength(this.loginMinLength)]],
            password: ['', [Validators.required, Validators.minLength(this.passwordMinLength)]],
        })
    }

    secureDataService = new SecureDataService();

    async onSubmit() : Promise<void>{
        if (this.loginForm.invalid)
            return;

        const {login: login, password: password} = this.loginForm.value

        console.log(`${login}-${password}`);

        const { salt, b } = await firstValueFrom(this.authApi.getSRPChallenge({ Login: login }));

        const srpProof = await this.secureDataService.generateSrpProof(password, salt, b);

        const { m2 } = await firstValueFrom(this.authApi.verifySRPProof({
          Login: login,
          A: srpProof.A,
          M1: srpProof.M1,
        }));

        if (!m2){
            this.errorMessage = "Ошибка аутентификации!"
            return;
        }

        const isValid = await this.secureDataService.verifyServerM2(
          srpProof.A,
          srpProof.M1,
          srpProof.S,
          m2
        );

        if (!isValid){
            this.errorMessage = "Подлинность сервера не подтверждена!"
            return;
        }

        alert('Вход успешен!');
        
        this.router.navigate(['/user']);

        this.isLoading = true;
        this.errorMessage = null;
    }
}