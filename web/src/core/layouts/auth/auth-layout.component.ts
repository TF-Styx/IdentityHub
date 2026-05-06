import { Component, ViewEncapsulation } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({selector: 'auth-layout', templateUrl: './auth-layout.component.html', styleUrls: ['./auth-layout.component.scss'], standalone: true, imports: [RouterOutlet], encapsulation: ViewEncapsulation.None})

export class AuthLayoutComponent {}