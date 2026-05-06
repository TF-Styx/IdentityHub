import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { GetSRPRequest, GetSRPResponse, VerifySRPRequest, VerifySRPResponse } from "./types";

@Injectable({providedIn: 'root'})

export class AuthApi{
    private httpClient: HttpClient = inject(HttpClient)
    private baseUrl: string = 'http://127.0.0.1:5077'

    getSRPChallenge = (login: GetSRPRequest) =>
        this.httpClient.post<GetSRPResponse>(`${this.baseUrl}/auth/srp/challenge`, login)

    verifySRPProof = (data: VerifySRPRequest) =>
        this.httpClient.post<VerifySRPResponse>(`${this.baseUrl}/auth/srp/verify`, data, {withCredentials: true})
}