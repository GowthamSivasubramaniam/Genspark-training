import { Component, OnInit, signal } from '@angular/core';
import { RecipeModel } from '../models/Recepi';
import { RecipeService } from '../service/RecepieService';
import { Recipe } from "../recipe/recipe";
import { CartItem } from '../models/cart';
import { debounceTime, distinctUntilChanged, Subject, switchMap } from 'rxjs';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-recipies',
  imports: [Recipe,FormsModule],
  templateUrl: './recipies.html',
  styleUrl: './recipies.css'
})
export class Recipies implements OnInit {
  recipes = signal<RecipeModel[]>([]);
  cartItems: CartItem[] = [];
  cartCount: number = 0;
  searchString: string = "";
  searchSubject = new Subject<string>();
  constructor(private recipeService: RecipeService) {

  }
  handleSearchProducts(){
    // console.log(this.searchString)
    this.searchSubject.next(this.searchString);
  }
  handleAddToCart(event: Number) {
    console.log("Handling add to cart - " + event)
    let flag = false;
    for (let i = 0; i < this.cartItems.length; i++) {
      if (this.cartItems[i].Id == event) {
        this.cartItems[i].Count++;
        flag = true;
      }
    }
    if (!flag)
      this.cartItems.push(new CartItem(event, 1));
    this.cartCount++;
  }
  ngOnInit(): void {
    this.recipeService.getAllRecipes().subscribe(
      {
        next: (data: any) => {
          this.recipes.set(data.recipes as RecipeModel[]);
        },
        error: (err) => { },
        complete: () => { }
      }
    )

     this.searchSubject.pipe(
      debounceTime(1000),
      distinctUntilChanged(),
      switchMap(query=>this.recipeService.getProductSearchResult(query))).subscribe({
        next:(data:any)=>{this.recipes.set(data.recipes as RecipeModel[]);}
      });
  }
}
