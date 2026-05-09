import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})

export class AvatarStateService {
    private file: File | null = null;

    readonly allowedTypes = ['image/png', 'image/jpeg'];
    readonly maxSize = 5 * 1024 * 1024;

    setFile(file: File): { [key: string]: any } | null {
        if (!this.allowedTypes.includes(file.type))
            return { invalidType: true };

        if (file.size > this.maxSize)
            return { fileTooLarge: true };

        this.file = file;

        return null;
    }

    getFile(): File | null {
        return this.file
    }

    resetState() {
        this.file = null;
    }
}