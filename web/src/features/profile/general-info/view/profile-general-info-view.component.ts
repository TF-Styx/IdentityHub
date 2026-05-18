import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";
import { ProfileGeneralInfoResponse } from "../../../../entities/user/model/types";

@Component({
    selector: 'app-profile-general-info-view',
    templateUrl: './profile-general-info-view.component.html',
    styleUrls: ['./profile-general-info-view.component.scss'],
    standalone: true,
    imports: [CommonModule]
})

export class ProfileGeneralInfoViewComponent {
    @Input() userData: Partial<ProfileGeneralInfoResponse> | null = null;
}