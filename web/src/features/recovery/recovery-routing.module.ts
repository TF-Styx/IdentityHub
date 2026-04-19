import { RouterModule, Routes } from "@angular/router";
import { RecoveryComponent } from "./recovery.component";
import { NgModule } from "@angular/core";
import { StepLoginComponent } from "./components/step-login/step-login.component";
import { StepCodeComponent } from "./components/step-code/step-code.component";
import { StepResetComponent } from "./components/step-reset/step-reset.component";
import { recoverStepGuard } from "./guards/recovery-step.guard";

const routes: Routes = [{
    path: '',
    component: RecoveryComponent,
    children: [
        { path: '', component: StepLoginComponent, canActivate: [recoverStepGuard] }, // /recovery
        { path: 'code', component: StepCodeComponent, canActivate: [recoverStepGuard] }, // /recovery/code
        { path: 'reset', component: StepResetComponent, canActivate: [recoverStepGuard] }, // /recovery/reset
        { path: '**', redirectTo: '' } // Fallback внутри модуля
    ]
}];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class RecoveryRoutingModule {}