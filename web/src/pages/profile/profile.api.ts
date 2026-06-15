import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ProfileGeneralInfoResponse, UpdateUserNameRequest } from "../../entities/user/model/types";

@Injectable({providedIn: 'root'})

export class ProfileApi {
    private httpClient: HttpClient = inject(HttpClient)

    getProfileGeneralInfo = (): Observable<ProfileGeneralInfoResponse> =>
        this.httpClient.get<ProfileGeneralInfoResponse>(`/general-info`);

    updateUserName = (request: UpdateUserNameRequest) : Observable<void> => 
        this.httpClient.patch<void>(`/update-name`, request);

    uploadAvatar = (avatar: FormData) : Observable<void> => 
        this.httpClient.post<void>(`/avatar`, avatar);
}