import { Component } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';


import { Login } from "./Components/login/login";
import { Register } from "./Components/register/register";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Login, Register],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'VehicleServiceManagement-FrontEnd';
  constructor(public router: Router) {}

  get showLayout(): boolean {
    const path = this.router.url;
    return path === '/login' || path === '/home' || path ==='/register';
  }
  showLogin:boolean=false
  showRegister:boolean=false
  cancel()
  {
    this.showLogin=false
  this.showRegister=false
  }
  showlogin()
  {
 this.showLogin=true
  }
  showregister()
  {
 this.showRegister=true
  }
  

}
