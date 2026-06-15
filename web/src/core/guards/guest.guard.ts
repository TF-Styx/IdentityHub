import { CanActivateFn, Router } from "@angular/router";
import { HttpClient, HttpErrorResponse, HttpStatusCode } from "@angular/common/http";
import { inject } from "@angular/core";
import { catchError, map, of } from "rxjs";

export const guestGuard: CanActivateFn = (route, state) => {
    const httpClient: HttpClient = inject(HttpClient);
    const router: Router = inject(Router);

    return httpClient.get
    (
        '/auth/status', 
        { 
            withCredentials: true, 
            headers: { 'X-Skip-Auth-Interceptor': 'true' } 
        }
    )
    .pipe(map(() => router.parseUrl('/user/profile')), 
    catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.Unauthorized)
            return of(true);
        
        return of(true);
    }));
}