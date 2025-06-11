import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-product',
  imports: [CommonModule],
  templateUrl: './product.html',
  styleUrl: './product.css'
})
export class Product {
products: { name: string; price: number;img:string }[];
count: number;

  constructor() {
    this.products = [
      { name: 'laptop', price: 10000,img:'/assets/photo-1484788984921-03950022c9ef.jpeg' },
      { name: 'mobile', price: 20000,img:'/assets/pexels-luckysam-47261.jpg' },
      { name: 'headphone', price: 3000 ,img:'/assets/download.jpeg'}
    ];
    this.count=0;
  }
  addcart()
  {
    this.count+=1;
  }

}
