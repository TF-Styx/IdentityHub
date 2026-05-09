import { Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})

export class AvatarStateService {
    private file: File | null = null;

    setFile(file: File) {
        this.file = file;
    }

    getFile(): File | null {
        return this.file
    }

    resetState() {
        this.file = null;
    }
}