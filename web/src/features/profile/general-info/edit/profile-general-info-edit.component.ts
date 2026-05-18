import { CommonModule } from "@angular/common";
import { Component, EventEmitter, Input, OnChanges, Output, SimpleChanges } from "@angular/core";
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from "@angular/forms";
import { ProfileGeneralInfoResponse } from "../../../../entities/user/model/types";
import { email } from "@angular/forms/signals";

@Component({
    selector: 'app-profile-general-info-edit',
    templateUrl: './profile-general-info-edit.component.html',
    styleUrls: ['./profile-general-info-edit.component.scss'],
    standalone: true,
    imports: [CommonModule, ReactiveFormsModule]
})

export class ProfileGeneralInfoEditComponent implements OnChanges {
    @Input() userData: Partial<ProfileGeneralInfoResponse> | null = null;
    @Output() save = new EventEmitter<ProfileGeneralInfoResponse>();
    @Output() cancel = new EventEmitter<void>();

    profileForm: FormGroup;

    constructor(private fb: FormBuilder) {
        this.profileForm = this.fb.group({
            login: [{ value: '', disable: true }],
            userName: ['', [Validators.required, Validators.minLength(2)]],
            email: ['', [Validators.email]]
        });
    }

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['userData']?.currentValue) {
            const data = changes['userData'].currentValue;
            this.profileForm.patchValue({
                login: data.login ?? '',
                userData: data.userData ?? '',
                email: data.email ?? '',
            }, 
            { emitEvent: false });
        }
    }

    onSubmit(): void {
        if (this.profileForm.valid) {
            const rawValue = this.profileForm.getRawValue();
            this.save.emit({
                login: rawValue.login,
                userName: rawValue.userName,
                email: rawValue.email
            });
        } else {
            this.profileForm.markAllAsTouched();
        }
    }

    onCancelClick(): void {
        this.cancel.emit();
    }
}