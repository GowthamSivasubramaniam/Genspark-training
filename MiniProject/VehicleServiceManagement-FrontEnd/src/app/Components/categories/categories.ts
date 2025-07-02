import { ChangeDetectorRef, Component } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { CategoryService } from '../../Services/CategoryService';
import { delay, switchMap } from 'rxjs';
import { CurrencyPipe } from '@angular/common';
import { nameValidator, priceValidator } from '../../Misc/Validations';

@Component({
  selector: 'app-categories',
  imports: [ReactiveFormsModule,FormsModule,CurrencyPipe],
  templateUrl: './categories.html',
  styleUrl: './categories.css'
})
export class Categories {
  CategoryForm: FormGroup;
  showform: boolean=false;
  showMessage:boolean=false;
  message:string=""
  showCategories: boolean=true;
  categories:any[]=[]
 constructor(private service:CategoryService,private cdr: ChangeDetectorRef)
 {
  this.CategoryForm = new FormGroup({
      Name: new FormControl(null, [Validators.required,nameValidator()]),
      Price: new FormControl("0.00", [Validators.required,priceValidator()])
    });


    this.service.getAllCategories().subscribe(
      {
        next:(data: any )=>
        {
        
  this.categories=data
        }
      }
    )
 }
 
 public get Name() : any {
  return  this.CategoryForm.get('Name')
 }
 public get Price() : any {
  return  this.CategoryForm.get('Price')
 }
 displayToast(msg: string) {
  this.message = msg;
  this.showMessage = true;
  
  setTimeout(() => {
    this.showMessage = false;
  }, 3000); 
}
  
 addCategory()
 {
   const data =
   {
    "name":this.Name.value,
    "price":this.Price.value
   }
   this.service.addcategory(data).subscribe(
   {
     next:  (data:any)=>
     {
     this.categories=[...this.categories,data]
       this.displayToast("Category added successfully");
      
        
        
  this.categories = [...this.categories, data];

        
      
     },
     error: (err:any)=>
     {
      console.log(err)
      this.displayToast(err.error.message)
     }
     
   })

 }
 updateCategory(price:number,id:string)
 {
    this.service.updateCategory(price,id).subscribe({
    next:(data:any)=>
    {
      this.displayToast("Updated Successfully")
    },
    error:(err:any)=>
    {
      this.displayToast(err.error.message)
    }
 })
}
 deleteCategory(id:string)
 {
 

   this.service.deleteCategory(id).subscribe({
     complete: () => {
 this.service.getAllCategories().subscribe(
      {
        next:(data: any )=>
        {
        
  this.categories=data
        }
      }
    )
     
      this.displayToast("Category updated successfully");
    },
    error:(err:any)=>
    {
      this.displayToast(err.error.message)
    }

   })
 }


  clickshowform() {
    this.showform = true
  
  }
  hideform() {
    this.showform = false
   this.showMessage = false;
  }

originalAmounts = new Map<string, number>();

startEditing(item: any) {
  item.isEditing = true;
  this.originalAmounts.set(item.categoryID, item.amount);
}

stopEditing(item: any) {
  item.isEditing = false;
  this.originalAmounts.delete(item.categoryID);
}

cancelEditing(item: any) {
  item.amount = this.originalAmounts.get(item.categoryID) ?? item.amount;
  this.stopEditing(item);
}
filter = {
  name: '',
  minPrice: null,
  maxPrice: null,
  status: ''
};

get filteredCategories() {
  return this.categories.filter(item =>
    (!this.filter.name || item.name.toLowerCase().includes(this.filter.name.toLowerCase())) &&
    (!this.filter.minPrice || item.amount >= this.filter.minPrice) &&
    (!this.filter.maxPrice || item.amount <= this.filter.maxPrice) &&
    (!this.filter.status || item.status.toLowerCase() === this.filter.status.toLowerCase())
  );
}


}
