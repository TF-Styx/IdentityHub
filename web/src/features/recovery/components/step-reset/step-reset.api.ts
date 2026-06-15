import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { PublicKeyResponse, RecoveryAccessPasswordRequest } from "./types";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})

export class StepResetApi{
    private httpClient: HttpClient = inject(HttpClient);
    
    getPublicKey = (): Observable<PublicKeyResponse> => 
        this.httpClient.get<PublicKeyResponse>(`/get-public-key`)

    recoveryAccessPassword = (data: RecoveryAccessPasswordRequest) =>
        this.httpClient.patch<void>(`/recovery-access-password`, data);
}