import { Component, inject } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { RecoveryStateService } from "../../services/recovery-state.service";

@Component({selector: 'app-step-code', templateUrl: './step-code.component.html', styleUrls: ['./step-code.component.scss'], standalone: true, imports: [ReactiveFormsModule]})

export class StepCodeComponent{
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);
    private route = inject(ActivatedRoute);
    private state = inject(RecoveryStateService);

    stepCodeForm: FormGroup;
    isLoading = false;
    errorMessage: string | null = null;

    constructor(){
        this.stepCodeForm = this.formBuilder.group({
            code: ['', [Validators.required]]
        });
    }

    onSubmit(){
        this.state.verifyCode(this.stepCodeForm.value.code);
        this.router.navigate(['reset'], {relativeTo: this.route.parent});
        // if (this.stepCodeForm.valid){
        //     const isValid = this.state.verifyCode(this.stepCodeForm.value.code);

        //     if (isValid)
        //         this.router.navigate(['/reset'], {relativeTo: this.route});
        //     else 
        //         this.stepCodeForm.setErrors({invalidCode: true});
        // }
    }
}
