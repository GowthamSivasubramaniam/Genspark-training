import { Component, OnInit, signal } from '@angular/core';
import { RecipeModel } from '../models/Recepi';
import { RecipeService } from '../service/RecepieService';
import { Recipe } from "../recipe/recipe";
import { CartItem } from '../models/cart';

@Component({
  selector: 'app-recipies',
  imports: [Recipe],
  templateUrl: './recipies.html',
  styleUrl: './recipies.css'
})
export class Recipies  implements OnInit {
  recipes = signal<RecipeModel[]>([]);
  cartItems:CartItem[] =[];
  cartCount:number =0;
  constructor(private recipeService:RecipeService){
   
  }
   handleAddToCart(event:Number)
  {
    console.log("Handling add to cart - "+event)
    let flag = false;
    for(let i=0;i<this.cartItems.length;i++)
    {
      if(this.cartItems[i].Id==event)
      {
         this.cartItems[i].Count++;
         flag=true;
      }
    }
    if(!flag)
      this.cartItems.push(new CartItem(event,1));
    this.cartCount++;
  }
  ngOnInit(): void {
    this.recipeService.getAllRecipes().subscribe(
      {
        next:(data:any)=>{
         this.recipes.set(data.recipes as RecipeModel[]);
        },
        error:(err)=>{},
        complete:()=>{}
      }
    )
  }
}
