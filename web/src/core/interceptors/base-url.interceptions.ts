import { HttpInterceptorFn } from "@angular/common/http";
import { environment } from "../environment/environment";

export const baseUrlInterceptions: HttpInterceptorFn = (request, next) => {
    if (request.url.startsWith('http://') || request.url.startsWith('https://')) 
        return next(request);

    const baseUrl = environment.baseUrl.replace(/\/$/, '');
    const pathRequest = request.url.replace(/^\//, '');

    const apiRequest = request.clone({url: `${baseUrl}/${pathRequest}`});

    return next(apiRequest);
}