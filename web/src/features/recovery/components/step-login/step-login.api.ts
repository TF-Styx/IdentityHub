import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})

export class StepLoginApi{
    private httpClient: HttpClient = inject(HttpClient);

    generateCode = (login: string) =>
        this.httpClient.post<void>(`/generate-code/${login}`, null);
}