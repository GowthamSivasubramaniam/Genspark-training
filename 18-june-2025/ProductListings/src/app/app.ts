import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Products } from "./products/products";
import { Navbar } from "./navbar/navbar";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, Navbar],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'ProductListings';
}
