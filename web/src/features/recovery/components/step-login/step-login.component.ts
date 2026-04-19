import { Component, inject } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router } from "@angular/router";
import { RecoveryStateService } from "../../services/recovery-state.service";

@Component({selector: 'app-step-login', templateUrl: './step-login.component.html', styleUrls: ['./step-login.component.scss'], standalone: true, imports: [ReactiveFormsModule]})

export class StepLoginComponent{
    private formBuilder = inject(FormBuilder);
    private router = inject(Router);
    private route = inject(ActivatedRoute);
    private state = inject(RecoveryStateService);

    stepLoginForm: FormGroup;
    isLoading = false;
    errorMessage: string | null = null;

    constructor(){
        this.stepLoginForm = this.formBuilder.group({
            login: ['', [Validators.required]]
        });
    }

    onSubmit(){
        if (this.stepLoginForm.valid){
            this.state.setLogin(this.stepLoginForm.value.login);
            this.router.navigate(['code'], {relativeTo: this.route});
        }
    }
}
