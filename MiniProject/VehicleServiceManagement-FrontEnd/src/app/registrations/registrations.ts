import { Component, HostListener } from '@angular/core';
import { registrationService } from '../Services/RegistrationService';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, catchError, debounceTime, delay, Observable, of, skip, Subject, switchMap, tap, throwError } from 'rxjs';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../Services/CategoryService';
import { BookingService } from '../Services/BookingService';
import { UserService } from '../Services/UserServices';
import { billService } from '../Services/BillService';
import { phoneValidator, priceValidator, vehicleNoValidator } from '../Misc/Validations';

@Component({
  selector: 'app-registrations',
  imports: [ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './registrations.html',
  styleUrl: './registrations.css'
})
export class Registrations {
vehicles: any[] = []
  services: any[] = []
  vehicleAddForm: FormGroup
  ServiceAddForm: FormGroup
  billForm: FormGroup
  showform: boolean = false
  message: string = ""
  showMessage: boolean = false
  searchSubject = new Subject<string>();
  ServiceSubject = new Subject<string>();
  loading: boolean = true;
  page = 1;
  role=""
  pageSize = 100;
  showBackToTop = false
  isVehicle = true
  vehicleQuery: string = ""
  serviceQuery: string = ""
  allcategories: string[] = []
  categories:any[]=[]
  constructor(private service: registrationService, private categoryService: CategoryService, private bookingService: BookingService, private userService: UserService,private BillService:billService) {
    
    this.role=this.userService.getRole();
    this.categoryService.getAllCategories().subscribe(
      {
        next: (data: any) => {
          data.forEach((element: any) => {
            if(element.status =="Active")
            this.allcategories.push(element.name)
          });

          console.log(this.allcategories)
        }
      })


    this.vehicleAddForm = new FormGroup(
      {
        No: new FormControl(null, [Validators.required,vehicleNoValidator()]),
        Type: new FormControl(null, [Validators.required]),
        Manufacturer: new FormControl(null, [Validators.required]),
        Model: new FormControl(null, [Validators.required]),
      })
    this.ServiceAddForm = new FormGroup(
      {
        VehicleNo: new FormControl(null, [Validators.required,vehicleNoValidator()]),
        Description: new FormControl(null, [Validators.required]),
        Customer_Phno: new FormControl(null, [Validators.required,phoneValidator()]),
        categorySearch:new FormControl(null),
        Categories: new FormControl([], [
  Validators.required,
  Validators.minLength(1) 
])

      })

    this.billForm = new FormGroup(
      {
        Amount: new FormControl(null, [Validators.required,priceValidator()]),

        billDescription: new FormControl(null, [Validators.required])

      })


    this.searchSubject.pipe(
      debounceTime(2000),
      tap(() => (this.loading = true)),

      switchMap(query =>
        this.service.showVehicles(query, this.page, this.pageSize).pipe(
          catchError(err => {
            this.showMessage = true;
            this.message = "No vehicles Found";
            return of([]);
          })
        )
      ),
     
      tap(() => (this.loading = false))

    ).subscribe(
      {
        next: (data: any) => {
          this.page = 1
          console.log(data)
          this.vehicles = data
        },
        error: (err: any) => {
          this.showMessage = true
          this.message = "No vehicles Found"
        }
      }
    )
    this.ServiceSubject.pipe(
      debounceTime(2000),
      tap(() => (this.loading = true)),

      switchMap(query =>
        this.service.showService(query, this.page, this.pageSize).pipe(
          catchError(err => {
            this.showMessage = true;
            this.message = "No Services Found";
            return of([]);
          })
        )
      ),
     
      tap(() => (this.loading = false))

    ).subscribe(
      {
        next: (data: any) => {
         
          this.page = 1
          console.log(data)
          this.services = data
        },
        error: (err: any) => {
          this.showMessage = true
          this.message = "No Services Found"
        }
      }
    )

   this.searchSubject.next("");   
  this.ServiceSubject.next("");  

    
  }




  public get No(): any {
    return this.vehicleAddForm.get("No");
  }
  public get Type(): any {
    return this.vehicleAddForm.get("Type");
  }
  public get Manufacturer(): any {
    return this.vehicleAddForm.get("Manufacturer");
  }
  public get Model(): any {
    return this.vehicleAddForm.get("Model");
  }
  public get VehicleNo(): any {
    return this.ServiceAddForm.get("VehicleNo");
  }

  public get Description(): any {
    return this.ServiceAddForm.get("Description");
  }
  public get Customer_Phno(): any {
    return this.ServiceAddForm.get("Customer_Phno");
  }
  public get categorySearch(): any {
    return this.ServiceAddForm.get("categorySearch");
  }
  
  public get billDescription(): any {
    return this.billForm.get("billDescription");
  }
  public get Amount(): any {
    return this.billForm.get("Amount");
  }






  clickshowform() {
    this.showform = true

  }
  hideform() {
    this.showform = false
    this.showServiceform=false

  }


  addVehicle() {
    const vehicle = {
      vehicleNo: this.No.value,
      vehicleType: this.Type.value,
      vechicleManufacturer: this.Manufacturer.value,
      vehicleModel: this.Model.value

    }

    this.service.addVehicle(vehicle).subscribe(
      {
        next: (data: any) => {
          this.showMessage = true;
          this.message = 'Vehicle added Successfully';
          this.vehicles = [...this.vehicles, data];

          this.vehicleAddForm.reset();
        },
        error: (err: any) => {
          this.showMessage = true;
          console.log(err)
          this.message = "Vehicle cannot be added";
        }
      }
    )
  }




  searchVehicles() {
    this.searchSubject.next(this.vehicleQuery);
  }
  searchServices() {
    this.ServiceSubject.next(this.serviceQuery);
  }


  loadMore() {
   
    if(this.isVehicle)
    {
       this.page += 1;
    this.service.showVehicles(this.vehicleQuery, this.page, this.pageSize).subscribe
      (
        {
          next: (data: any) => {

            this.vehicles.push(...data);
          },
          error: (err: any) => {
            this.page = 1
            this.showMessage = true
            this.message = err.error.message
          }
        }
      )
  }
  else
  {
     this.page += 1;
    this.service.showService(this.serviceQuery, this.page, this.pageSize).subscribe
      (
        {
          next: (data: any) => {

            this.services.push(...data);
          },
          error: (err: any) => {
            this.page = 1
            this.showMessage = true
            this.message = err.error.message
          }
        }
      )
  }
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








  filteredCategories: any
  filterCategories() {
    const search = this.categorySearch.value.toLowerCase();
    console.log(this.categorySearch)
    this.filteredCategories = this.allcategories.filter(cat =>
      cat.toLowerCase().includes(search)
    );
  }

  addCategory(name: string) {
    const current = this.ServiceAddForm.value.Categories;
    if (name && !current.includes(name)) {
      this.ServiceAddForm.patchValue({ Categories: [...current, name] });
    }

    this.filterCategories();
  }

  removeCategory(name: string) {
    const current = this.ServiceAddForm.value.Categories;
    this.ServiceAddForm.patchValue({ Categories: current.filter((c: string) => c !== name) });
  }


  booking: any[] = []
  bookingId:string=""
  addService() {

    this.showMessage = false;
    this.message = '';
    const nowHourString = new Date().toISOString().slice(0, 13);
    const email = localStorage.getItem("email") ?? "";

    this.bookingService.getallActiveBookings().pipe(
      switchMap((bookings: any[]) => {
        const filtered = bookings.filter(b =>
          b.customer.phone === this.Customer_Phno.value 
          // && b.slot.slice(0, 13) === nowHourString
        );

        if (filtered.length === 0) return throwError(() => new Error("No matching bookings found"));

        const BookingID = filtered[0].bookingID;
        this.bookingId=BookingID
        const CustomerID = filtered[0].customerID;

        return this.service.showVehicles(this.VehicleNo.value, this.page, this.pageSize).pipe(
          switchMap((vehicles: any[]) => {
            if (vehicles.length === 0) return throwError(() => new Error("No vehicles found"));
            if(vehicles.length >1) return throwError(() => new Error("Multiple Vehicles Found with the no , Kindly type the full number"));

            const VehicleID = vehicles[0].vehicleID;

            const serviceData = {
              vehicleID: VehicleID,
              description: this.Description.value,


              categoryNames: this.ServiceAddForm.value.Categories
            };

            return this.service.addService(serviceData).pipe(
              switchMap((serviceRes: any) => {
                const ServiceID = serviceRes.serviceID;

                return this.userService.getProfile(email).pipe(
                  switchMap(() => {
                    const MechanicID = this.userService.getId();

                    const serviceRecordData = {
                      mechanicId: MechanicID,
                      customerID: CustomerID,
                      serviceID: ServiceID,
                      bookingID: BookingID
                    };
                    console.log(serviceRecordData)

                    return this.service.addServiceRecord(serviceRecordData)
                      ;
                  })
                );
              })
            );
          })
        );
      })
    ).subscribe({
      next: (data:any) => {
        this.services = [...this.services, data];
         this.bookingService.UpdateBooking(this.bookingId).subscribe(
          {
            next:(data:any)=>
            {
              console.log("Reviewed")
            },
            error:(err:any)=>
            {
              console.log(err)
            }
          }
         )
        this.showMessage = true;
        this.message = "Added Successfully";
        
         this.ServiceAddForm.reset();
      },
      error: (err: any) => {

        this.showMessage = true;
        console.error("Service record creation failed:", err);
        this.message = err.error?.message || err?.message || "Something went wrong";
      }
    });



  }

showServiceform=false
  clickshowServiceform()
  {
    this.showServiceform=true;
  }

  setview(value:boolean)
  {
    this.isVehicle=value
    if(!this.isVehicle)
    {
      this.ServiceSubject.next(this.serviceQuery)
    }
    else
    {
      this.searchSubject.next(this.vehicleQuery)
    }

  }
  showBillForm =false
  item:any
   serviceRecordId:string=""
  onStatusChange(item: any, newStatus: string,id:string) {
    if(newStatus== "Completed")
    {
      this.serviceRecordId=id
      this.item=item
      this.showBillForm =true
      
    }
    else
    {
    this.service.updateStatus(item.serviceRecordID,newStatus).subscribe
    (
      {
        next:(data:any)=>
        {
             item.status = newStatus;
        },
        error:(err:any)=>
        {
                   this.showMessage = true;
        this.message = err.error?.message || err?.message || "Something went wrong";
        }
      }
    )
  }
  
  
  }
  getStatusClass(status: string): string {
  switch (status) {
    case 'Active': return 'active';
    case 'Completed': return 'completed';
    case 'Aborted': return 'aborted';
    default: return 'active';
  }
}


addBill()
{
  const data =
    {
  "serviceRecordID":  this.serviceRecordId,
  "miscAmount": this.Amount.value,
  "description": this.billDescription.value
}
this.BillService.addBill(data).subscribe
({
  next:(data:any)=>
  {
    console.log(data)
    this.message="Bill Dispatched Successfully"
    this.billForm.reset();
    this.showMessage=true;
     this.service.updateStatus(this.serviceRecordId,"Completed").subscribe
    (
      {
        next:(data:any)=>
        {
             this.item.status = "Completed";
        },
        error:(err:any)=>
        {
                   this.showMessage = true;
        this.message = err.error?.message || err?.message || "Something went wrong";
        }
      }
    )
    this.showBillForm=false
  },
  error:(err:any)=>
  {
    this.showMessage=true;
    console.error(err)
    this.message=err.error.message || err.message || "Something went Wrong";

}}
)
}


filters = {
  vehicleType: '',
  manufacturer: '',
  model: ''
};

get filteredVehicles() {
  return this.vehicles.filter(vehicle =>
    (!this.filters.vehicleType || vehicle.vehicleType === this.filters.vehicleType) &&
    (!this.filters.manufacturer || vehicle.vechicleManufacturer.toLowerCase().includes(this.filters.manufacturer.toLowerCase())) &&
    (!this.filters.model || vehicle.vehicleModel.toLowerCase().includes(this.filters.model.toLowerCase()))
  );
}
filter = {
  vehicleNo: '',
  customerName: '',
  email: '',
  mechanicName: '',
  mechanicEmail: this.role=="Mechanic"? localStorage.getItem("email"):'' ,
  category: '',
  status: ''
};

get filteredRecords() {
  return this.services.filter(item =>
    (!this.filter.vehicleNo || item.vehicleNo.toLowerCase().includes(this.filter.vehicleNo.toLowerCase())) &&
    (!this.filter.customerName || item.customerName.toLowerCase().includes(this.filter.customerName.toLowerCase())) &&
    (!this.filter.email || item.customer_Email.toLowerCase().includes(this.filter.email.toLowerCase())) &&
    (!this.filter.mechanicName || item.mechanicName.toLowerCase().includes(this.filter.mechanicName.toLowerCase())) &&
    (!this.filter.mechanicEmail || item.mechanic_Email.toLowerCase().includes(this.filter.mechanicEmail.toLowerCase())) &&
    (!this.filter.status || item.status === this.filter.status) &&
    (!this.filter.category || item.categories.some((cat: string) =>
      cat.toLowerCase().includes(this.filter.category.toLowerCase())
    ))
  );
}

}


