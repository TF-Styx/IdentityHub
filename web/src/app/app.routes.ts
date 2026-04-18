import { Routes } from '@angular/router';
import { AuthComponent } from '../features/auth/auth.component'
import { RegisterComponent } from '../features/register/register.component';

export const routes: Routes = 
    [
        {path: 'auth', component: AuthComponent}, 
        {path: 'register', component: RegisterComponent},
        {path: '', redirectTo: '/auth', pathMatch: 'full'},
    ];
