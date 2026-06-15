import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";

@Injectable({providedIn: 'root'})

export class StepCodeApi{
    private httpClient: HttpClient = inject(HttpClient);

    verifyConfirmCode = (login: string, code: number) =>
        this.httpClient.post<void>(`/verify-confirm-code/${login}/${code}`, null);
}