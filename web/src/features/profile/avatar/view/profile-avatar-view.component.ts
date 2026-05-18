import { CommonModule } from "@angular/common";
import { Component, Input } from "@angular/core";

@Component({
    selector: 'app-profile-avatar-view',
    templateUrl: './profile-avatar-view.component.html',
    styleUrls: ['./profile-avatar-view.component.scss'],
    standalone: true,
    imports: [CommonModule]
})

export class ProfileAvatarViewComponent {
    @Input() avatarUrl: string | null = null;
}