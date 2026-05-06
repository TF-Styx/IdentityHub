import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators, ɵInternalFormsSharedModule, ReactiveFormsModule } from "@angular/forms";
import { ProfileGeneralInfoApi } from "./profile-general-info.api";

@Component({selector: 'general-info', templateUrl: './profile-general-info.component.html', styleUrls: ['./profile-general-info.component.scss'], standalone: true, imports: [CommonModule, ɵInternalFormsSharedModule, ReactiveFormsModule]})

export class ProfileGeneralInfoComponent implements OnInit {
    private formBuilder: FormBuilder = inject(FormBuilder)
    private http: ProfileGeneralInfoApi = inject(ProfileGeneralInfoApi)
    private changeDetectorRef: ChangeDetectorRef = inject(ChangeDetectorRef)

    profileForm: FormGroup;
    errorMessage: string | null = null;

    isLoading: boolean = false

    private readonly userNameMinLength = 2;

    constructor() {
        this.profileForm = this.formBuilder.group({
            login: ['', []],
            userName: ['', [Validators.required, Validators.minLength(this.userNameMinLength)]],
            email: ['', []]
        });
    }
    
    ngOnInit(): void {
        this.getProfileGeneralInfo()
    }

    getProfileGeneralInfo(): void {
        this.http.getProfileGeneralInfo()
        .subscribe({
            next: response => {
                this.profileForm.patchValue(response);
                this.isLoading = true;
                this.changeDetectorRef.detectChanges();
            }, 
            error: errors => {
                console.log(errors);
                this.isLoading = true;
            }
        });
    }

    async onUpdateUserName(): Promise<void> {
        if (this.profileForm.invalid)
            return;

        this.http.updateUserName({userName: this.profileForm.value.userName})
            .subscribe({
                next: () => alert('Изменение имени пользователя прошло успешно!'),
                error: errors => console.log(errors)
                
            });
    }
}