import { CommonModule } from "@angular/common";
import { Component } from "@angular/core";
import { ProfileGeneralInfoComponent } from "../../features/profile/general-info/profile-general-info.component";
import { RecoveryRoutingModule } from "../../features/recovery/recovery-routing.module";

@Component({
    selector: 'app-profile-page',
    standalone: true,
    imports: [CommonModule, ProfileGeneralInfoComponent],
    templateUrl: './profile.page.html',
})

export class ProfilePage{
    
}