import { HttpInterceptorFn } from "@angular/common/http";

export const credentialsInterceptions: HttpInterceptorFn = (request, next) => {
    const cloneRequest = request.clone({withCredentials: true});
    return next(cloneRequest);
}