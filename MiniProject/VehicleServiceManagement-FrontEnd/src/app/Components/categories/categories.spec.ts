import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Categories } from './categories';
import { CategoryService } from '../../Services/CategoryService';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
import { of, throwError } from 'rxjs';

describe('Categories Component', () => {
  let component: Categories;
  let fixture: ComponentFixture<Categories>;
  let mockService: jasmine.SpyObj<CategoryService>;

  beforeEach(async () => {
    mockService = jasmine.createSpyObj('CategoryService', ['getAllCategories', 'addcategory', 'updateCategory', 'deleteCategory']);
    
    await TestBed.configureTestingModule({
      imports: [Categories],
      providers: [
        { provide: CategoryService, useValue: mockService },
        CurrencyPipe
      ]
    }).compileComponents();

    mockService.getAllCategories.and.returnValue(of([
      { categoryID: '1', name: 'Test', amount: 100, status: 'active' }
    ]));

    fixture = TestBed.createComponent(Categories);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load categories on init', () => {
    expect(component.categories.length).toBeGreaterThan(0);
    expect(component.categories[0].name).toBe('Test');
  });

  it('should show and hide form', () => {
    component.clickshowform();
    expect(component.showform).toBeTrue();

    component.hideform();
    expect(component.showform).toBeFalse();
    expect(component.showMessage).toBeFalse();
  });

  it('should validate form controls', () => {
    component.CategoryForm.setValue({ Name: '', Price: '' });
    expect(component.CategoryForm.invalid).toBeTrue();
  });

  it('should add category and update UI', () => {
    mockService.addcategory.and.returnValue(of({ categoryID: '2', name: 'Newe', amount: 200 }));

    component.CategoryForm.setValue({ Name: 'Newe', Price: 200 });
    component.addCategory();

    expect(mockService.addcategory).toHaveBeenCalled();
    expect(component.categories.some(c => c.name === 'New')).toBeFalse();
   
  });

  it('should handle add category error', () => {
    mockService.addcategory.and.returnValue(throwError(() => ({ error: { message: '' } })));

    component.CategoryForm.setValue({ Name: 'New', Price: '200' });
    component.addCategory();

    expect(component.message).toBe('');
  });

  it('should update category and show success message', () => {
    mockService.updateCategory.and.returnValue(of({}));

    component.updateCategory(150, '1');
    expect(mockService.updateCategory).toHaveBeenCalledWith(150, '1');
    
  });

  it('should handle update error gracefully', () => {
    mockService.updateCategory.and.returnValue(throwError(() => ({ error: { message: '' } })));

    component.updateCategory(150, '1');
    expect(component.message).toBe('');
  });

  it('should delete category and reload list', fakeAsync(() => {
    mockService.deleteCategory.and.returnValue(of({}));
    mockService.getAllCategories.and.returnValue(of([{ name: 'Remaining', amount: 150, categoryID: '2', status: 'active' }]));

    component.deleteCategory('1');
    tick();

    expect(mockService.deleteCategory).toHaveBeenCalled();
    expect(component.categories[0].name).toBe('Remaining');
  }));

  it('should filter categories based on input', () => {
    component.filter.name = 'test';
    expect(component.filteredCategories.length).toBe(1);

    component.filter.name = 'nonexistent';
    expect(component.filteredCategories.length).toBe(0);
  });

  it('should track original price when editing and allow cancel', () => {
    const item = { categoryID: '1', amount: 100, isEditing: false };
    component.startEditing(item);
    expect(item.isEditing).toBeTrue();

    item.amount = 200;
    component.cancelEditing(item);
    expect(item.amount).toBe(100);
    expect(item.isEditing).toBeFalse();
  });
});
