import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})

export class StepLoginApi{
    private httpClient: HttpClient = inject(HttpClient);
    private baseUrl: string = 'http://127.0.0.1:5077';

    generateCode = (login: string) =>
        this.httpClient.post<void>(`${this.baseUrl}/generate-code/${login}`, null);
}