import { Component, OnInit, signal } from '@angular/core';
import { RecipeModel } from '../models/recipe';
import { RecipeService } from '../services/ReceipeService';
import { Recipe } from "../recipe/recipe";

@Component({
  selector: 'app-recipes',
  imports: [Recipe],
  templateUrl: './recipes.html',
  styleUrl: './recipes.css'
})
export class Recipes implements OnInit {
  recipes = signal<RecipeModel[]>([]);
  constructor(private recipeService:RecipeService){

  }
  ngOnInit(): void {
    this.recipeService.getAllRecipes().subscribe(
      {
        next:(data:any)=>{
          console.log(data.recipes)
         this.recipes.set(data.recipes as RecipeModel[]);
        }
      }
    )
  }
}