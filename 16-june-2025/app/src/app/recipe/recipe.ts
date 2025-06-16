import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { RecipeModel } from '../models/Recepi';
import { RecipeService } from '../service/RecepieService';

@Component({
  selector: 'app-recipe',
  imports: [],
  templateUrl: './recipe.html',
  styleUrl: './recipe.css'
})
export class Recipe {
@Input() recipe:RecipeModel|null = new RecipeModel();
@Output() addToCart:EventEmitter<Number> = new EventEmitter<Number>();
private recipieservice = inject(RecipeService);

handleBuyClick(pid:Number|undefined){
  if(pid)
  {
      console.log(pid);
      this.addToCart.emit(pid);
  }
}
}
