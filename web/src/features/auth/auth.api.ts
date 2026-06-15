import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { CompleteSRPRequest, GetSRPRequest, GetSRPResponse, VerifySRPRequest, VerifySRPResponse } from "./types";

@Injectable({providedIn: 'root'})

export class AuthApi{
    private httpClient: HttpClient = inject(HttpClient)

    getSRPChallenge = (login: GetSRPRequest) =>
        this.httpClient.post<GetSRPResponse>(`/auth/srp/challenge`, login)

    verifySRPProof = (data: VerifySRPRequest) =>
        this.httpClient.post<VerifySRPResponse>(`/auth/srp/verify`, data)

    completeSRP = (data: CompleteSRPRequest) => 
        this.httpClient.post('/auth/srp/complete', data)
}