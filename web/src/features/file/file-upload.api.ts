import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable({providedIn: 'root'})

export class UploadApi {
    private httpClient: HttpClient = inject(HttpClient)
    private baseUrl: string = 'http://127.0.0.1:5077'

    uploadAvatar = (avatar: FormData) : Observable<void> => 
        this.httpClient.post<void>(`${this.baseUrl}/avatar`, avatar, {withCredentials: true});
}