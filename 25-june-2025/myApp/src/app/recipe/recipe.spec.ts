import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipe } from './recipe';
import { RecipeModel } from '../models/recipe';

describe('Recipe Component', () => {
  let component: Recipe;
  let fixture: ComponentFixture<Recipe>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Recipe], 
    }).compileComponents();

    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render recipe ', () => {
    const s: RecipeModel = {
      id: 1,
      name: 'Chocolate Cake',
      cookTimeMinutes: 100,
      image: 'a.jpg',
     
    };

    component.recipe = s;
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('1');
  });

  
});
