import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ProfileGeneralInfoResponse, UpdateUserNameRequest } from "../../entities/user/model/types";

@Injectable({providedIn: 'root'})

export class ProfileApi {
    private httpClient: HttpClient = inject(HttpClient)
    private baseUrl: string = 'http://127.0.0.1:5077'

    getProfileGeneralInfo = (): Observable<ProfileGeneralInfoResponse> =>
        this.httpClient.get<ProfileGeneralInfoResponse>(`${this.baseUrl}/general-info`, {withCredentials: true});

    updateUserName = (request: UpdateUserNameRequest) : Observable<void> => 
        this.httpClient.patch<void>(`${this.baseUrl}/update-name`, request, {withCredentials: true});

    uploadAvatar = (avatar: FormData) : Observable<void> => 
        this.httpClient.post<void>(`${this.baseUrl}/avatar`, avatar, {withCredentials: true});
}