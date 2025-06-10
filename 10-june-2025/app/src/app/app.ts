import { Component } from '@angular/core';
import { First } from "./first/first";
import { Customer } from './customer/customer';
import { Product } from './product/product';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Customer , Product]
})
export class App {
  protected title = 'App';
}