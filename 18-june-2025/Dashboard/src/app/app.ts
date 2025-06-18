import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Register } from "./register/register";
import { Navbar } from "./navbar/navbar";
import { Dashboard } from "./dashboard/dashboard";
import { MapComponent } from "./map/map";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar, MapComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'Dashboard';
}
