import { HttpClient } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";

@Injectable()
export class RecipeService{
    private http = inject(HttpClient);

    getAllRecipes():Observable<any[]>{
        return this.http.get<any[]>('https://dummyjson.com/recipe');
    }
     getProductSearchResult(searchData:string)
    {
        return this.http.get('https://dummyjson.com/recipe/search?q='+searchData)
    }
}