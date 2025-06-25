import { TestBed } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RecipeService } from './ReceipeService';

describe('RecipeService', () => {
  let service: RecipeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [RecipeService]
    });

    service = TestBed.inject(RecipeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); 
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should fetch all recipes using GET', () => {
    const mockRecipes = [
      { id: 1, title: 'Recipe 1' },
    ];

    service.getAllRecipes().subscribe((data) => {
      expect(data).toEqual(mockRecipes);
    });

    const req = httpMock.expectOne('https://dummyjson.com/recipe');
    expect(req.request.method).toBe('GET');
    req.flush(mockRecipes);
  });
});
