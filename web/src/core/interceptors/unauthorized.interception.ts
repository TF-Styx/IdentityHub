import { HttpErrorResponse, HttpInterceptorFn, HttpStatusCode } from "@angular/common/http";
import { inject } from "@angular/core";
import { Router } from "@angular/router";
import { catchError, throwError } from "rxjs";

export const unauthorizedInterception: HttpInterceptorFn = (request, next) => {
    const router: Router = inject(Router);

    if (request.headers.has('X-Skip-Auth-Interceptor'))
        return next(request);

    return next(request).pipe(
        catchError((error: HttpErrorResponse) => {
            if (error.status === HttpStatusCode.Unauthorized)
                router.navigate(['/auth'], { queryParams: { returnUrl: request.url } });

            return throwError(() => error);
        })
    );
}