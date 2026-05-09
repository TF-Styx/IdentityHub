import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { AvatarStateService } from "./services/avatar-state.service";

@Component({selector: 'app-file-upload', templateUrl: './file-upload.component.html', styleUrls: ['./file-upload.component.scss'], standalone: true, imports: [CommonModule]})

export class FileUploadComponent {
    private avatarState: AvatarStateService = inject(AvatarStateService);

    selectedFile: File | null = null;
    imagePreviewUrl: string | null = null;

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        const file = input.files?.[0];

        if (!file) 
            return;

        if (this.imagePreviewUrl) {
            URL.revokeObjectURL(this.imagePreviewUrl);
        }

        this.selectedFile = file;
        this.imagePreviewUrl = URL.createObjectURL(file);
        this.avatarState.setFile(file);
    }

    clearSelection(): void {
        if (this.imagePreviewUrl) {
            URL.revokeObjectURL(this.imagePreviewUrl);
        }
        this.selectedFile = null;
        this.imagePreviewUrl = null;
        
        const input = document.querySelector('input[type="file"]') as HTMLInputElement;
        if (input) 
            input.value = '';
    }

    ngOnDestroy(): void {
        if (this.imagePreviewUrl)
            URL.revokeObjectURL(this.imagePreviewUrl);
    }
}