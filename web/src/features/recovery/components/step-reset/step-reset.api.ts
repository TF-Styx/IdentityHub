import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { PublicKeyResponse, RecoveryAccessPasswordRequest } from "./types";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})

export class StepResetApi{
    private httpClient: HttpClient = inject(HttpClient);
    private baseUrl: string = 'http://127.0.0.1:5077';
    
    getPublicKey = (): Observable<PublicKeyResponse> => 
        this.httpClient.get<PublicKeyResponse>(`${this.baseUrl}/get-public-key`)

    recoveryAccessPassword = (data: RecoveryAccessPasswordRequest) =>
        this.httpClient.patch<void>(`${this.baseUrl}/recovery-access-password`, data);
}