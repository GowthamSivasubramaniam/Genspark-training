import { Component, inject } from '@angular/core';
import { ProductModel } from '../models/product';
import { ProductService } from '../../Services/ProductService';
import { ActivatedRoute } from '@angular/router';
import { CurrencyPipe } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CurrencyPipe],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
  
  product:any|undefined
  router = inject(ActivatedRoute)
  id = this.router.snapshot.params['id'] as number
  constructor(productService:ProductService)
  {
    console.log("hii")
    productService.getProductById(this.id).subscribe(
      { next:(data:any) =>
      {
           this.product = data;
           console.log(this.product);
      }
    }
    )
    
  }


}
