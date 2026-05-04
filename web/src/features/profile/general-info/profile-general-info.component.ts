import { CommonModule } from "@angular/common";
import { ChangeDetectorRef, Component, inject, OnInit } from "@angular/core";
import { FormBuilder, ɵInternalFormsSharedModule } from "@angular/forms";
import { ProfileGeneralInfoApi } from "./profile-general-info.api";

@Component({selector: 'general-info', templateUrl: './profile-general-info.component.html', styleUrls: ['./profile-general-info.component.scss'], standalone: true, imports: [CommonModule, ɵInternalFormsSharedModule]})

export class ProfileGeneralInfoComponent implements OnInit {
    private fromBuilder: FormBuilder = inject(FormBuilder)
    private http: ProfileGeneralInfoApi = inject(ProfileGeneralInfoApi)
    private changeDetectorRef: ChangeDetectorRef = inject(ChangeDetectorRef)

    login: string | null = null
    userName: string | null = null
    email: string | null = null

    isLoading: boolean = false
    
    ngOnInit(): void {
        this.getProfileGeneralInfo()
    }

    getProfileGeneralInfo(): void {
        this.http.getProfileGeneralInfo()
            .subscribe
            ({
                next: response => 
                {
                    this.login = response.login,
                    this.userName = response.userName,
                    this.email = response.email,

                    this.isLoading = true
                    this.changeDetectorRef.detectChanges()
                }, 
                error: errors => console.log(errors)
            })
    }
}