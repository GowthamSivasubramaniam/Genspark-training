import { Component } from '@angular/core';
import { First } from "./first/first";
import { Customer } from './customer/customer';
import { Product } from './product/product';
import { Pro } from "./pro/pro";
import { Recipies } from './recipies/recipies';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  styleUrl: './app.css',
  imports: [Recipies]
})
export class App {
  protected title = 'App';
}