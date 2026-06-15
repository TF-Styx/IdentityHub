import { ApplicationConfig, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { baseUrlInterceptions } from '../core/interceptors/base-url.interceptions';
import { credentialsInterceptions } from '../core/interceptors/credentials.interceptions';
import { unauthorizedInterception } from '../core/interceptors/unauthorized.interception';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([baseUrlInterceptions, credentialsInterceptions, unauthorizedInterception])),
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes)
  ]
};
