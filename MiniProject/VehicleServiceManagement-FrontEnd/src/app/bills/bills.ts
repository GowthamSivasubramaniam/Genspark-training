import { Component, HostListener } from '@angular/core';
import { catchError, debounceTime, delay, of, Subject, switchMap, tap } from 'rxjs';
import { billService } from '../Services/BillService';
import { registrationService } from '../Services/RegistrationService';
import { CommonModule, CurrencyPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { UserService } from '../Services/UserServices';

@Component({
  selector: 'app-bills',
  imports: [CurrencyPipe,CommonModule,FormsModule],
  templateUrl: './bills.html',
  styleUrl: './bills.css'
})
export class Bills {
  message: string = ""
  showMessage: boolean = false
  searchSubject = new Subject<string>();
  toast: string = ""; 
  loading: boolean = true;
  page = 1;
  role: string = ""
  pageSize = 100;
  showBackToTop = false

  query: string = ""

  bills:any[] = []
  constructor( private service:billService,private userService:UserService ,private registrationService:registrationService) {
  this.role = userService.getRole();

  this.searchSubject.pipe(
    debounceTime(2000),
    tap(() => (this.loading = true)),
    switchMap(query =>
      this.service.showAllBills(query, this.page, this.pageSize).pipe(
        catchError(err => {
          this.toast = "error";
          this.showMessage = true;
          this.message = "No Bills Found";
          return of([]);
        })
      )
    ),
    tap(() => (this.loading = false))
  ).subscribe(
    {
      next: (data: any) => {
        this.toast = "success";
        this.showMessage = false;
        
        this.page = 1
        this.bills = data
        if(this.role=="Customer" )
        {
          this.bills=this.bills.filter( b => b.email === localStorage.getItem("email") && (b.status== "Approved" || b.status== "Paid"))
        }
        if(this.role=="Mechanic" )
        {
          this.bills=this.bills.filter( b => b.memail === localStorage.getItem("email"))
        }
      },
      error: (err: any) => {
        this.toast = "error";
        this.showMessage = true
        this.message = "No Bills Found"
      }
    }
  )

  this.searchSubject.next("");   
  }





  searchBills() {
    this.searchSubject.next(this.query);
  }
 downloadBills(id:string) {
  this.service.download(id).subscribe(
    {
      next: (blob) => {
        this.toast = "success";
        this.showMessage = false;
        setTimeout(() => {
          this.showMessage = true;
          this.message = "Download successful";
        }, 1000);
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `bill_${id}.pdf`;
        a.click();
        window.URL.revokeObjectURL(url);
      },
      error: (err) => {
        this.toast = "error";
        this.showMessage = false;
        setTimeout(() => {
          this.showMessage = true;
          this.message = "Download failed";
        }, 1000);
        console.error('Download failed:', err);
      }
    }
  )
}

loadMore() {
  this.page += 1;
  this.service.showAllBills(this.query, this.page, this.pageSize).subscribe(
    {
      next: (data: any) => {
        this.toast = "success";
        this.showMessage = false;
        setTimeout(() => {
          this.showMessage = true;
          this.message = "More bills loaded";
        }, 1000);
        this.bills.push(...data);
      },
      error: (err: any) => {
        this.toast = "error";
        this.page = 1
        this.showMessage = false;
        setTimeout(() => {
          this.showMessage = true;
          this.message = err.error.message || "Failed to load more bills";
        }, 1000);
      }
    }
  )
}

  @HostListener('window:scroll', [])
  onScroll(): void {

    this.showBackToTop = window.scrollY > 300;
    const scrollPosition = window.innerHeight + window.scrollY;
    console.log(scrollPosition)
    const threshold = document.body.offsetHeight;
    console.log(threshold)
    if (scrollPosition >= threshold+30) {
      this.loadMore();

    }


  }
  scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }




onStatusChange(item: any, newStatus: string) {
  if(newStatus!="Decline")
  {
    this.service.updateStatus(item.billID,newStatus).subscribe(
      {
        next:(data:any) => {
          this.toast = "success";
          this.showMessage = false;
          setTimeout(() => {
            this.showMessage = true;
            this.message = "Status updated successfully";
          }, 1000);
          item.status = newStatus;
        },
        error:(err:any) => {
          this.toast = "error";
          this.showMessage = false; 
          setTimeout(() => {
            this.showMessage = true; 
            this.message = err.error?.message || err?.message || "Something went wrong";
          },1000)
        }
      }
    )
  }
  else
  {
    this.service.deleteBill(item.billID).subscribe(
      {
        next:(data:any) => {
          this.toast = "success";
          this.showMessage = false;
          setTimeout(() => {
            this.showMessage = true;
            this.message = "Declined Successfully";
          }, 1000);
          this.registrationService.updateStatus(data.serviceRecordID,"Active").subscribe(
            {
              next:(data:any) => {
                this.toast = "success";
                this.showMessage = false;
                setTimeout(() => {
                  this.showMessage = true;
                  this.message = "Service record re-activated";
                }, 1000);
                this.bills = this.bills.filter(bill => bill.billID !== item.billID);
              }
            }
          )
        },
        error:(err:any) => {
          this.toast = "error";
          this.showMessage = false; 
          setTimeout(() => {
            this.showMessage = true; 
            this.message = err.error?.message || err?.message || "Something went wrong";
          },1000)
        }
      }
    )
  }

  
  
  
  }
  getStatusClass(status: string): string {
  switch (status) {
    case 'Dispatched': return 'dispatched';
    case 'Approved': return 'approved';
    case 'Decline': return 'decline';
    default: return 'Dispatched';
  }
}



filters = {
  vehicleNo: '',
  customerName: '',
  mechanicName: '',
  category: '',
  description: '',
  status: '',
  minAmount: null,
  maxAmount: null
};

get filteredBills() {
  return this.bills.filter(item =>
    (!this.filters.vehicleNo || item.vehicleNo.toLowerCase().includes(this.filters.vehicleNo.toLowerCase())) &&
    (!this.filters.customerName || item.customerName.toLowerCase().includes(this.filters.customerName.toLowerCase())) &&
    (!this.filters.mechanicName || item.mechanicName.toLowerCase().includes(this.filters.mechanicName.toLowerCase())) &&
    (!this.filters.description || item.description.toLowerCase().includes(this.filters.description.toLowerCase())) &&
    (!this.filters.category || item.categoryAmounts.some((cat: any) =>
      cat.categoryName.toLowerCase().includes(this.filters.category.toLowerCase())
    )) &&
    (!this.filters.status || item.status === this.filters.status) &&
    (this.filters.minAmount == null || item.totalAmount >= this.filters.minAmount) &&
    (this.filters.maxAmount == null || item.totalAmount <= this.filters.maxAmount)
  );
}



}


