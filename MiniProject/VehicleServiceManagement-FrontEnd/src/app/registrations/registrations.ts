import { Component, HostListener } from '@angular/core';
import { registrationService } from '../Services/RegistrationService';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { BehaviorSubject, catchError, debounceTime, delay, Observable, of, skip, Subject, switchMap, tap, throwError } from 'rxjs';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../Services/CategoryService';
import { BookingService } from '../Services/BookingService';
import { UserService } from '../Services/UserServices';
import { billService } from '../Services/BillService';
import { manufacturerValidator, modelYearValidator, phoneValidator, priceValidator, vehicleNoValidator } from '../Misc/Validations';

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
  toast: string = ""; // Add this variable at the top with other properties
  searchSubject = new Subject<string>();
  ServiceSubject = new Subject<string>();
  loading: boolean = true;
  page = 1;
  role = ""
  pageSize = 100;
  showBackToTop = false
  isVehicle = true
  vehicleQuery: string = ""
  serviceQuery: string = ""
  allcategories: string[] = []
  categories: any[] = []
  noitems = false
  phoneNo:string=""
  expandedItem:any=null
  email:string=localStorage.getItem("email")||""
  constructor(private service: registrationService, private categoryService: CategoryService, private bookingService: BookingService, private userService: UserService, private BillService: billService) {

    this.role = this.userService.getRole();
    this.categoryService.getAllCategories().subscribe(
      {
        next: (data: any) => {
          data.forEach((element: any) => {
            if (element.status == "Active")
              this.allcategories.push(element.name)
          });
            
          console.log(this.allcategories)
        }
      })
      
       this.userService.getProfile(this.email).subscribe(
      {
        next:(data:any)=>
        {
          if(this.role=="Customer" || this.role=="Mechanic")
          {
          this.phoneNo=data.phone
          console.log(this.phoneNo)
          this.ServiceSubject.next(data.phone);
          }
        }
      });

    this.vehicleAddForm = new FormGroup(
      {
        No: new FormControl(null, [Validators.required, vehicleNoValidator()]),
        Type: new FormControl("Two Wheeler", [Validators.required]),
        Manufacturer: new FormControl(null, [Validators.required,manufacturerValidator()]),
        Model: new FormControl(null, [Validators.required,modelYearValidator()])
      })
    this.ServiceAddForm = new FormGroup(
      {
        VehicleNo: new FormControl(null, [Validators.required, vehicleNoValidator()]),
        Description: new FormControl(null, [Validators.required, Validators.minLength(4)]),
        Customer_Phno: new FormControl(null, [Validators.required, phoneValidator()]),
        categorySearch: new FormControl(null),
        Categories: new FormControl([], [
          Validators.required,
          Validators.minLength(1)
        ])

      })

    this.billForm = new FormGroup(
      {
        Amount: new FormControl(null, [Validators.required, priceValidator()]),

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
          if(this.role=="Mechanic")
          {
            this.services=this.services.filter(s=>s.mechanic_PhoneNo == this.phoneNo)
          }
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
    this.showServiceform = false

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
          this.toast = "success";
          this.showMessage = false;
          setTimeout(() => {
            this.showMessage = true;
            this.message = 'Vehicle added Successfully';
          }, 1000);
          this.vehicles = [...this.vehicles, data];
          this.vehicleAddForm.reset();
        },
        error: (err: any) => {
          this.toast = "error";
          console.log(err)
          this.showMessage = false;
          setTimeout(() => {
            this.showMessage = true;
            this.message = err.error.message || "Vehicle cannot be added";
          }, 1000)
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
    if (this.isVehicle) {
      this.page += 1;
      this.service.showVehicles(this.vehicleQuery, this.page, this.pageSize).subscribe(
        {
          next: (data: any) => {
            this.toast = "success";
            this.showMessage = false;
            setTimeout(() => {
              this.showMessage = true;
              this.message = "More vehicles loaded";
            }, 1000);
            this.vehicles.push(...data);
          },
          error: (err: any) => {
            this.toast = "error";
            this.page = 1
            this.showMessage = false;
            setTimeout(() => {
              this.showMessage = true;
              this.message = err.error.message;
            }, 1000);
          }
        }
      )
    }
    else {
      this.page += 1;
      this.service.showService(this.serviceQuery, this.page, this.pageSize).subscribe(
        {
          next: (data: any) => {
            this.toast = "success";
            this.showMessage = false;
            setTimeout(() => {
              this.showMessage = true;
              this.message = "More services loaded";
            }, 1000);
            this.services.push(...data);
          },
          error: (err: any) => {
            this.toast = "error";
            this.page = 1
            this.showMessage = false;
            setTimeout(() => {
              this.showMessage = true;
              this.message = err.error.message;
            }, 1000);
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
    if (scrollPosition >= threshold) {
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
  bookingId: string = ""
  addService() {

    this.showMessage = false;
    this.message = '';
    const nowHourString = new Date().toISOString().slice(0, 13);
    const email = localStorage.getItem("email") ?? "";

    this.bookingService.getallActiveBookings().pipe(
      switchMap((bookings: any[]) => {
        const filtered = bookings.filter(b =>
          b.customer.phone === this.Customer_Phno.value
        );

        if (filtered.length === 0) return throwError(() => new Error("No matching bookings found"));

        const BookingID = filtered[0].bookingID;
        this.bookingId = BookingID
        const CustomerID = filtered[0].customerID;

        return this.service.showVehicles(this.VehicleNo.value, this.page, this.pageSize).pipe(
          switchMap((vehicles: any[]) => {
            if (vehicles.length === 0) return throwError(() => new Error("No vehicles found"));
            if (vehicles.length > 1) return throwError(() => new Error("Multiple Vehicles Found with the no , Kindly type the full number"));

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

                    return this.service.addServiceRecord(serviceRecordData);
                  })
                );
              })
            );
          })
        );
      })
    ).subscribe({
      next: (data: any) => {
        this.toast = "success";
        this.showMessage = false;
        setTimeout(() => {
          this.showMessage = true;
          this.message = "Added Successfully";
        }, 1000);
        this.services = [...this.services, data];
        this.bookingService.UpdateBooking(this.bookingId).subscribe(
          {
            next: (data: any) => {
              this.toast = "success";
              
            },
            error: (err: any) => {
              this.toast = "error";
              
            }
          }
        )
        this.ServiceAddForm.reset();
      },
      error: (err: any) => {
        this.toast = "error";
        this.showMessage = false;
        setTimeout(() => {
          this.showMessage = true;
          this.message = err.error?.message || err?.message || "Something went wrong";
        }, 1000)
      }
    });



  }

  showServiceform = false
  clickshowServiceform() {
    this.showServiceform = true;
  }

  setview(value: boolean) {
    this.isVehicle = value
    if (!this.isVehicle) {
      this.ServiceSubject.next(this.serviceQuery)
    }
    else {
      this.searchSubject.next(this.vehicleQuery)
    }

  }
  showBillForm = false
  item: any
  serviceRecordId: string = ""
  onStatusChange(item: any, newStatus: string, id: string) {
    if (newStatus == "Completed") {
      this.serviceRecordId = id
      this.item = item
      this.showBillForm = true

    }
    else {
      this.service.updateStatus(item.serviceRecordID, newStatus).subscribe
        (
          {
            next: (data: any) => {
              this.toast = "success";
              item.status = newStatus;
            },
            error: (err: any) => {
              this.toast = "error";
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


  addBill() {
    const data =
    {
      "serviceRecordID": this.serviceRecordId,
      "miscAmount": this.Amount.value,
      "description": this.billDescription.value
    }
    this.BillService.addBill(data).subscribe
      ({
        next: (data: any) => {
          this.toast = "success";
          this.showMessage = false;
          setTimeout(() => {
            this.showMessage = true;
            this.message = "Bill Dispatched Successfully";
          }, 1000);
          this.billForm.reset();
          this.service.updateStatus(this.serviceRecordId, "Completed").subscribe(
            {
              next: (data: any) => {
                this.toast = "success";
                this.item.status = "Completed";
              },
              error: (err: any) => {
                this.toast = "error";
                this.showMessage = true;
                this.message = err.error?.message || err?.message || "Something went wrong";
              }
            }
          )
          this.showBillForm = false
        },
        error: (err: any) => {
          this.toast = "error";
          this.showMessage = false;
          setTimeout(() => {
            this.message = "Bill for this service is already available";
            this.showMessage = true;
          }, 1000);
          console.error(err)
        }
      }
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
    mechanicEmail: this.role == "Mechanic" ? localStorage.getItem("email") : '',
    category: '',
    status: ''
  };

  get filteredRecords() {
     console.log(this.filter.mechanicEmail )
    return this.services
  .filter(item =>
    (!this.filter.vehicleNo || item.vehicleNo.toLowerCase().includes(this.filter.vehicleNo.toLowerCase())) &&
    (!this.filter.customerName || item.customerName.toLowerCase().includes(this.filter.customerName.toLowerCase())) &&
    (!this.filter.email || item.customer_Email.toLowerCase().includes(this.filter.email.toLowerCase())) &&
    (!this.filter.mechanicName || item.mechanicName.toLowerCase().includes(this.filter.mechanicName.toLowerCase())) &&
    (!this.filter.mechanicEmail || item.mechanic_Email.toLowerCase().includes(this.filter.mechanicEmail.toLowerCase())) &&
    (!this.filter.status || item.status === this.filter.status) &&
    (!this.filter.category || item.categories.some((cat: string) =>
      cat.toLowerCase().includes(this.filter.category.toLowerCase())
    ))
  )
  .sort((a, b) => {
  const order = {
    'active': 0,
    'completed': 1,
    'aborted': 2
  };

  const aStatus = a.status?.toLowerCase() as keyof typeof order;
  const bStatus = b.status?.toLowerCase() as keyof typeof order;
  const aPriority = order[aStatus] ?? 99;
  const bPriority = order[bStatus] ?? 99;

  return aPriority - bPriority;
});

  }
  toggleCard(item: any) {
    this.expandedItem = this.expandedItem == item ? null : item;
  }
}


