import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { RegisterRequest, PublicKeyResponse } from "./types";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})

export class RegisterApi{
    private httpClient: HttpClient = inject(HttpClient);
    private baseUrl: string = 'http://127.0.0.1:5077'

    getPublicKey = (): Observable<PublicKeyResponse> => 
        this.httpClient.get<PublicKeyResponse>(`${this.baseUrl}/get-public-key`)

    registration = (data: RegisterRequest): Observable<void> =>
        this.httpClient.post<void>(`${this.baseUrl}/registration`, data)
}