import { Component, inject } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { RecoveryStateService } from "../../services/recovery-state.service";
import { StepCodeApi } from "./step-code.api";
import { firstValueFrom } from "rxjs";

@Component({selector: 'app-step-code', templateUrl: './step-code.component.html', styleUrls: ['./step-code.component.scss'], standalone: true, imports: [ReactiveFormsModule]})

export class StepCodeComponent{
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);
    private route = inject(ActivatedRoute);
    private state = inject(RecoveryStateService);
    private stepCodeApi = inject(StepCodeApi);
    private recoveryState = inject(RecoveryStateService);

    stepCodeForm: FormGroup;
    isLoading = false;
    errorMessage: string | null = null;

    constructor(){
        this.stepCodeForm = this.formBuilder.group({
            code: ['', [Validators.required]]
        });
    }

    async onSubmit(): Promise<void>{
        if (this.stepCodeForm.valid){
            try{
                await firstValueFrom(this.stepCodeApi.verifyConfirmCode(this.recoveryState.login!, this.stepCodeForm.value.code));
                
                this.state.verifyCode(this.stepCodeForm.value.code);
                this.router.navigate(['reset'], {relativeTo: this.route.parent});
                
            } catch (err){
                console.error('Ошибка: ', err);
            }
        }
    }
}
