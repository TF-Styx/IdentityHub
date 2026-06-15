import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { RegisterRequest, PublicKeyResponse } from "./types";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})

export class RegisterApi{
    private httpClient: HttpClient = inject(HttpClient);

    getPublicKey = (): Observable<PublicKeyResponse> => 
        this.httpClient.get<PublicKeyResponse>(`/get-public-key`)

    registration = (data: RegisterRequest): Observable<void> =>
        this.httpClient.post<void>(`/registration`, data)
}