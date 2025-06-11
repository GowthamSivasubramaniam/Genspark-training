import { Component, inject } from '@angular/core';
import { ProductService } from '../service/productservice';
import { ProductModel } from '../models/product';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-pro',
  imports: [CurrencyPipe],
  templateUrl: './pro.html',
  styleUrl: './pro.css'
})
export class Pro {
product:ProductModel|null = new ProductModel();
private productService = inject(ProductService);

constructor(){
    this.productService.getProduct(1).subscribe(
      {
        next:(data)=>{
          this.product = data as ProductModel;
          console.log(this.product)
        },
        error:(err)=>{
          console.log(err)
        },
        complete:()=>{
          console.log("All done");
        }
      })
}
}
