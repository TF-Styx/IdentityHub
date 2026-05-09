import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, inject, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators, ɵInternalFormsSharedModule, ReactiveFormsModule } from "@angular/forms";
import { ProfileGeneralInfoApi } from "./profile-general-info.api";
import { FileUploadComponent } from "../../file/file-upload.component";
import { UploadApi } from "../../file/file-upload.api";
import { AvatarStateService } from "../../file/services/avatar-state.service";
import { firstValueFrom } from "rxjs";

@Component({selector: 'general-info', templateUrl: './profile-general-info.component.html', styleUrls: ['./profile-general-info.component.scss'], standalone: true, imports: [CommonModule, ɵInternalFormsSharedModule, ReactiveFormsModule, FileUploadComponent]})

export class ProfileGeneralInfoComponent implements OnInit {
    private formBuilder: FormBuilder = inject(FormBuilder);
    private http: ProfileGeneralInfoApi = inject(ProfileGeneralInfoApi);
    private changeDetectorRef: ChangeDetectorRef = inject(ChangeDetectorRef);
    private fileUploadApi: UploadApi = inject(UploadApi);
    private avatarStateService: AvatarStateService = inject(AvatarStateService);

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
        
        const file = this.avatarStateService.getFile();

        if (file == null) {
            alert('Файл не был выбран!');
            return;
        }
        
        const fileName: string = file?.name as string;

        const fromData = new FormData();
        fromData.append('File', file, fileName);
        console.log(`File - ${file}, FileName - ${fileName}, FromData - ${fromData}`);

        await firstValueFrom(this.fileUploadApi.uploadAvatar(fromData));
    }
}