import { CommonModule } from "@angular/common";
import { Component, ElementRef, EventEmitter, inject, Input, OnChanges, OnDestroy, Output, SimpleChanges, ViewChild } from "@angular/core";

@Component({selector: 'app-file-upload', templateUrl: './file-upload.component.html', styleUrls: ['./file-upload.component.scss'], standalone: true, imports: [CommonModule]})

export class FileUploadComponent implements OnChanges, OnDestroy {
    @Input() currentImageUrl: string | null = null;
    @Output() fileSelected = new EventEmitter<File>();
    @Output() fileCleared = new EventEmitter<void>();

    @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

    selectedFile: File | null = null;
    imagePreviewUrl: string | null = null;

    ngOnChanges(changes: SimpleChanges): void {
        if (changes['currentImageUrl'] && !this.selectedFile) {
            this.cleanupPreview()
            this.imagePreviewUrl = changes['currentImageUrl'].currentValue
        }
    }

    onFileSelected(event: Event): void {
        const input = event.target as HTMLInputElement;
        const file = input.files?.[0];

        if (!file) 
            return;

        this.cleanupPreview();

        this.selectedFile = file;
        this.imagePreviewUrl = URL.createObjectURL(file);

        this.fileSelected.emit(file);
    }

    clearSelection(): void {
        this.cleanupPreview();

        this.selectedFile = null;
        this.imagePreviewUrl = this.currentImageUrl;

        this.fileInput.nativeElement.value = '';
        this.fileCleared.emit();
    }

    private cleanupPreview(): void {
        if (this.imagePreviewUrl?.startsWith('blob:'))
            URL.revokeObjectURL(this.imagePreviewUrl);
    }

    ngOnDestroy(): void {
        this.cleanupPreview();
    }
}