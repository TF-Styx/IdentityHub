import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { AvatarStateService } from "./services/avatar-state.service";

@Component({selector: 'app-file-upload', templateUrl: './file-upload.component.html', styleUrls: ['./file-upload.component.scss'], standalone: true, imports: [CommonModule]})

export class FileUploadComponent {
    private avatarState: AvatarStateService = inject(AvatarStateService);

    selectedFile: File | null = null;
    imagePreviewUrl: string | null = null;

    // constructor(private uploadService: Upload)

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        const file = input.files?.[0];

        if (!file) 
            return;

        // ✅ Валидация
        const allowedTypes = ['image/png', 'image/jpeg'];
        if (!allowedTypes.includes(file.type)) {
            alert('Разрешены только PNG и JPEG');
            input.value = ''; // сброс инпута
            return;
        }
        if (file.size > 5 * 1024 * 1024) {
            alert('Максимальный размер: 5 МБ');
            input.value = '';
            return;
        }

        // ⚠️ Очищаем предыдущий URL, чтобы не было утечки памяти
        if (this.imagePreviewUrl) {
            URL.revokeObjectURL(this.imagePreviewUrl);
        }

        this.selectedFile = file;
        this.imagePreviewUrl = URL.createObjectURL(file); // Генерируем URL для <img>
        this.avatarState.setFile(file);
    }

    clearSelection(): void {
        if (this.imagePreviewUrl) {
            URL.revokeObjectURL(this.imagePreviewUrl);
        }
        this.selectedFile = null;
        this.imagePreviewUrl = null;
        
        // Сбрасываем значение скрытого инпута (чтобы можно было выбрать тот же файл повторно)
        const input = document.querySelector('input[type="file"]') as HTMLInputElement;
        if (input) 
            input.value = '';
    }

    ngOnDestroy(): void {
        if (this.imagePreviewUrl)
            URL.revokeObjectURL(this.imagePreviewUrl);
    }
}