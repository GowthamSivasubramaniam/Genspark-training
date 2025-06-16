import { Component, HostListener } from '@angular/core';
import { ProductModel } from '../models/product';
import { ProductService } from '../../Services/ProductService';
import { debounceTime, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { FormsModule } from '@angular/forms';
import { CurrencyPipe } from '@angular/common';
@Component({
  selector: 'app-products',
  imports: [FormsModule,CurrencyPipe],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class Products {
   products:ProductModel[] = [];
   limit =10;
   skip=0;
   max=0;
   searchQuery:string="";
   loading:Boolean = false;
   showBackToTop = false;
   private searchSubject = new Subject<string>();
   constructor(private productService:ProductService){}
   handleSearch()
   {
     this.searchSubject.next(this.searchQuery)
   }
   ngOnInit() : void
   {
      this.searchSubject.pipe(
        debounceTime(1000),
        distinctUntilChanged(),
        tap(()=> this.loading = true),
        switchMap(query => this.productService.getProductsWithSearch(query,this.limit,this.skip)),
        tap(()=> this.loading = false),
      )
      .subscribe(
        { next:(data:any) =>
        {
          this.products = []
          this.products = data.products as ProductModel[];
          this.skip=0;
          this.max= data.total;
        }
      }
      );
      this.handleSearch()
   }
  @HostListener('window:scroll',[])
  onScroll():void
  {
    
    this.showBackToTop = window.scrollY > 300;
    const scrollPosition = window.innerHeight + window.scrollY;
    const threshold = document.body.offsetHeight-200;
    if(scrollPosition>=threshold && this.products?.length<this.max)
    {
      this.loadMore();
      
    }
    
  }
  loadMore()
  {
    this.loading = true;
    this.skip += this.limit;
    this.productService.getProductsWithSearch(this.searchQuery,this.limit,this.skip).subscribe
    (
         { next:(data:any) =>
         {
             this.products.push(...data.products);

             this.loading = false
         }
      }

    )
  }
  getHighlightedTitle(title: string): string {
  if (!this.searchQuery) return title;
  const pattern = this.searchQuery
  const regex = new RegExp(`(${pattern})`, 'gi');
  return title.replace(regex, `<mark>$1</mark>`);
}
scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

}
