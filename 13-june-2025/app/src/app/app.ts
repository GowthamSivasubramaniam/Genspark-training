import { Component } from '@angular/core';
import { Login } from "./login/login";
import { Welcome } from "./welcome/welcome";
import { Pubsublogin } from "./pubsublogin/pubsublogin";
import { Listenwelocome } from "./listenwelocome/listenwelocome";

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Login, Welcome, Pubsublogin, Listenwelocome]
})
export class App {
  protected title = 'App';
}