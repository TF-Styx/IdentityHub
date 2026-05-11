import { CommonModule } from "@angular/common";
import { Component, EventEmitter, inject, Input, OnChanges, OnInit, Output, SimpleChanges } from "@angular/core";
import { FormBuilder, FormGroup, Validators, ɵInternalFormsSharedModule, ReactiveFormsModule } from "@angular/forms";
import { ProfileGeneralInfoResponse } from "./types";

@Component({
    selector: 'general-info', 
    templateUrl: './profile-general-info.component.html', 
    styleUrls: ['./profile-general-info.component.scss'], 
    standalone: true, 
    imports: [CommonModule, ɵInternalFormsSharedModule, ReactiveFormsModule]
})

export class ProfileGeneralInfoComponent implements OnChanges {
    @Input() userData: Partial<ProfileGeneralInfoResponse> | null = null;
    @Output() save = new EventEmitter<ProfileGeneralInfoResponse>();
    @Output() cancel = new EventEmitter<void>();

    profileForm: FormGroup;

    constructor(private fb: FormBuilder) {
        this.profileForm = this.fb.group({
            login: ['', []],
            userName: ['', [Validators.required, Validators.minLength(2)]],
            email: ['', [Validators.email]]
        });
    }
    
    ngOnChanges(changes: SimpleChanges): void {
        if (changes['userData'] && this.userData) {
            this.profileForm.patchValue({
                login: this.userData.login || '',
                userName: this.userData.userName || '',
                email: this.userData.email || ''
            }, 
            { emitEvent: false });
        }
    }

    onSubmit(): void {
        if (this.profileForm.valid) {
            this.save.emit(this.profileForm.getRawValue());
        }
    }

    onCancelClick(): void {
        this.cancel.emit();
    }
}