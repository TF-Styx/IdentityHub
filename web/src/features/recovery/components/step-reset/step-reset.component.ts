import { Component, inject } from "@angular/core";
import { AbstractControl, FormBuilder, FormGroup, ReactiveFormsModule, ValidationErrors, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { RecoveryStateService } from "../../services/recovery-state.service";

@Component({selector: 'app-step-reset', templateUrl: './step-reset.component.html', styleUrls: ['./step-reset.component.scss'], standalone: true, imports: [ReactiveFormsModule]})

export class StepResetComponent{
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);
    private route = inject(ActivatedRoute);
    private state = inject(RecoveryStateService);

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

    onSubmit(){
        if (this.stepResetForm){
            alert('Пароль успешно изменён!');
            this.state.resetState();
            this.router.navigate(['/auth']);
        }
    }
}
