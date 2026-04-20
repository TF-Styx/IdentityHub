import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})

export class StepCodeApi{
    private httpClient: HttpClient = inject(HttpClient);
    private baseUrl: string = 'http://127.0.0.1:5077'

    verifyConfirmCode = (login: string, code: number) =>
        this.httpClient.post<void>(`${this.baseUrl}/verify-confirm-code/${login}/${code}`, null);
}