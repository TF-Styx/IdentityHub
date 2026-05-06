import { Routes } from '@angular/router';
import { AuthComponent } from '../features/auth/auth.component'
import { RegisterComponent } from '../features/register/register.component';
import { AuthLayoutComponent } from '../core/layouts/auth/auth-layout.component';
import { RecoveryRoutingModule } from '../features/recovery/recovery-routing.module';
import { MainLayoutComponent } from '../core/layouts/main/main-layout.component';
import { ProfilePage } from '../pages/profile/profile.page';

export const routes: Routes = 
    [
        {path: '', redirectTo: '/auth', pathMatch: 'full'},
        {
            path: '', 
            loadComponent: () => AuthLayoutComponent, 
            children: 
            [
                {path: 'auth', loadComponent: () => AuthComponent},
                {path: 'register', loadComponent: () => RegisterComponent},
                {path: 'recovery', loadChildren: () => RecoveryRoutingModule}
            ]
        },

        {
            path: 'user',
            loadComponent: () => MainLayoutComponent,
            children:
            [
                {path: 'profile', loadComponent: () => ProfilePage},
                {path: '', redirectTo: 'profile', pathMatch: 'full'}
            ]
        },
    ];
