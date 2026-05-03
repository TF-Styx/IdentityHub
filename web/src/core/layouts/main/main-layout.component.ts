import { Component } from "@angular/core";
import { RouterOutlet } from "@angular/router";

@Component({selector: 'main-layout', templateUrl: './main-layout.component.html', styleUrls: ['./main-layout.component.scss'], standalone: true, imports: [RouterOutlet]})

export class MainLayoutComponent {}