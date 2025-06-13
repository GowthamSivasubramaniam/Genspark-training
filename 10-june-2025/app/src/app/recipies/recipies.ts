import { Component, OnInit, signal } from '@angular/core';
import { RecipeModel } from '../models/Recepi';
import { RecipeService } from '../service/RecepieService';
import { Recipe } from "../recipe/recipe";

@Component({
  selector: 'app-recipies',
  imports: [Recipe],
  templateUrl: './recipies.html',
  styleUrl: './recipies.css'
})
export class Recipies  implements OnInit {
  recipes = signal<RecipeModel[]>([]);
  constructor(private recipeService:RecipeService){

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
