import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipes } from './recipes';

import { of } from 'rxjs';
import { Recipe } from '../recipe/recipe';
import { RecipeModel } from '../models/recipe';
import { RecipeService } from '../services/ReceipeService';

describe('Recipies Component', () => {
  let fixture: ComponentFixture<Recipes>;
  let component: Recipes;


  const mockRecipes: RecipeModel[] = [
    {
        id: 1,
      name: 'Chocolate Cake',
      cookTimeMinutes: 100,
      image: 'a.jpg',
    }
  ];

  
  const mockRecipeService = {
    getAllRecipes: jasmine.createSpy('getAllRecipes').and.returnValue(of({ recipes: mockRecipes }))
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Recipes, Recipe], 
      providers: [
        { provide: RecipeService, useValue: mockRecipeService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Recipes);
    component = fixture.componentInstance;
    fixture.detectChanges(); 
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should fetch and store recipes using signals', () => {
    const r = component.recipes();
    expect(r.length).toBe(1);
  });

  it('should render recipe child components', () => {
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain(' Cake');

  });
});
