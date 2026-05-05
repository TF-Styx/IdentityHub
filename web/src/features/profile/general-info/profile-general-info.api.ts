import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ProfileGeneralInfoResponse, UpdateUserNameRequest } from "./types";

@Injectable({providedIn: 'root'})

export class ProfileGeneralInfoApi {
    private httpClient: HttpClient = inject(HttpClient)
    private baseUrl: string = 'http://127.0.0.1:5077'

    getProfileGeneralInfo = (): Observable<ProfileGeneralInfoResponse> =>
        this.httpClient.get<ProfileGeneralInfoResponse>(`${this.baseUrl}/general-info`, {withCredentials: true});

    updateUserName = (request: UpdateUserNameRequest) : Observable<void> => 
        this.httpClient.patch<void>(`${this.baseUrl}/update-name`, request, {withCredentials: true});
}