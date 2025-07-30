import { Component, ElementRef, ViewChild } from '@angular/core';
import { Booking } from '../../models/BookingsModel';
import { FormControl, FormGroup, FormsModule, Validators } from '@angular/forms';
import { BookingService } from '../../Services/BookingService';
import { UserService } from '../../Services/UserServices';
import { Store } from '@ngrx/store';
import { selectAllBookings } from '../../Store/BookingStore/booking.selector';
import * as BookingActions from '../../Store/BookingStore/booking.actions';
import { delay, distinctUntilChanged, Subject, switchMap, tap } from 'rxjs';
import { CommonModule, NgOptimizedImage } from '@angular/common';

@Component({
  selector: 'app-bookings',
  imports: [CommonModule,FormsModule],
  templateUrl: './bookings.html',
  styleUrl: './bookings.css'
})
export class Bookings {
  @ViewChild('fileInput') fileInput!: ElementRef;

  bookingForm: FormGroup;
  filteredBookings: Booking[] = [];
  slots: string[] = [];
  id: any = null;
  showMessage = false;
  message = '';
  toast: string = ""; 
  bookings: Booking[] = []
  slotFormValue: string = '';
  showform: boolean = false
  isBooked: boolean = false
  loading: boolean = false
  role: string = ""
  showbookings: boolean = true
  private subject = new Subject<string>();
  constructor(private service: BookingService, private userService: UserService, private store: Store) {
    this.role = userService.getRole();
    console.log(this.role)
    const email = localStorage.getItem('email') || '';
    this.userService.getProfile(email).pipe(
      tap(() => this.loading = true)
    ).subscribe({
      next: () => {
        this.id = this.userService.getId();
       this.store.dispatch(BookingActions.loadBookings());
        this.subject.next('');
      
      },
    })
      ;


    this.subject.pipe(
      tap(() => this.loading = true),
      distinctUntilChanged(),
      switchMap(() => this.store.select(selectAllBookings)),
    
      tap(() => this.loading = false),
    )
      .subscribe(
        {
          next: (data: any) => {

            this.bookings = data
            if (this.role === "Customer") {
              this.bookings = this.bookings
                .filter(b => b.customerID === this.id)
                };

               const bookedCount = this.bookings.filter(b => b.status === "Booked").length;
               if(bookedCount>=1 && this.role === "Customer")
               {
                this.isBooked=true
               }
              
               this.applyFilter()


            }

          }
      )

    console.log(this.id)
    this.bookingForm = new FormGroup({
      phone:new FormControl("", [Validators.required]),
      slot: new FormControl(null, [Validators.required]),
      image: new FormControl(null, [Validators.required])
    });

    this.service.getslots().subscribe({
      next: (data: any) => {
        


        this.slots = data.filter((slot: string) => {
          const [datePart, timePart] = slot.split(' ');
          const [day, month, year] = datePart.split('/').map(Number);
          const [hours, minutes] = timePart.split(':').map(Number);

          const slotDateTime = new Date(year, month - 1, day, hours, minutes);
          const now = new Date();
          const hourSlice = now.toISOString().slice(0, 13);
          const slotSlice = slotDateTime.toISOString().slice(0, 13);
     
          return slotSlice >= hourSlice;
        });

      }
    });
  }

  public get slot() {
    return this.bookingForm.get('slot');
  }

  public get image() {
    return this.bookingForm.get('image');
  }

  selectSlot(slotValue: string) {
    this.slot?.setValue(slotValue);
    this.slot?.markAsTouched();
    this.slotFormValue = slotValue;
  }

  onImageSelected(event: Event) {
    const file = (event.target as HTMLInputElement).files?.[0];
    if (file) {
      this.image?.setValue(file);
      this.image?.markAsTouched();
    }
  }

  book() {
    console.log("hii")
    const formData = new FormData();
    formData.append('CustomerID', this.id);
    formData.append('Slot', this.bookingForm.get('slot')?.value);

    const file = this.bookingForm.get('image')?.value;
    if (file) {
      formData.append('Image', file, file.name);
    }
    this.service.createBooking(formData).subscribe({
      next: (data:any) => {
        this.toast = "success";
        this.showMessage = false;
        setTimeout(() => {
          this.showMessage = true;
          this.message = 'Booking Successful';
        }, 1000);
        this.store.dispatch(BookingActions.loadBookings());
        this.bookingForm.reset();
        this.slotFormValue = '';
        this.fileInput.nativeElement.value = '';
        this.showform = false
      },
      error: (err:any) => {
        this.toast = "error";
        console.log(err)
        this.slotFormValue = '';
        this.showMessage = false; 
        setTimeout(() => {
          this.showMessage = true; 
          this.message = err?.error?.message ||" Booking failed. Please try again.";
        },1000)
        this.bookingForm.reset();
        this.slotFormValue = '';
        this.fileInput.nativeElement.value = '';
      }
    });
  }
  getFileNameFromPath(path: string): string {
    if (!path) return '';
    return path.split('/').pop() || '';
  }

  clickshowform() {
     if(this.isBooked)
    {
      this.toast = "error";
         this.showMessage = false; 
  setTimeout(() => {
             this.showMessage = true; 

      this.message="Already Booked a slot "
    }, 1000)
    }
    else
    {
    this.showform = true
    }

  }
  clickback() {
    this.showform = false

  }
  showCancelMessage = false
  cancelBooking(itemid: any, index: number) {
    this.service.cancelBooking(itemid).subscribe(
      {
        next: (data: any) => {
          this.toast = "success";
          this.showCancelMessage = false;
          setTimeout(() => {
            this.showCancelMessage = true;
            this.message = "cancelled successfully"
          }, 1000);
          this.bookings = this.bookings.map(b =>
            b.bookingID === itemid ? new Booking({ ...b, status: 'Cancelled' }) : b
          );
           this.store.dispatch(BookingActions.loadBookings());
          this.isBooked = false
        },
        error: (err: any) => {
          this.toast = "error";
          this.showCancelMessage = true;
          this.message = err?.error?.message || err?.message || "Cancel failed";
        }
      }
    )
  }
filter = {
  name: '',
  email: '',
  phone: '',
  status: '',
  slotFrom: '',
  slotTo: '',
  deliveryFrom: '',
  deliveryTo: ''
};



applyFilter() {
  this.filteredBookings = this.bookings.filter(b => {
    const slot = new Date(b.slot).getTime();
    const delivery = new Date(b.deliveryTime).getTime();

    const slotFrom = this.filter.slotFrom ? new Date(this.filter.slotFrom).getTime() : null;
    const slotTo = this.filter.slotTo ? new Date(this.filter.slotTo).getTime() : null;

    const deliveryFrom = this.filter.deliveryFrom ? new Date(this.filter.deliveryFrom).getTime() : null;
    const deliveryTo = this.filter.deliveryTo ? new Date(this.filter.deliveryTo).getTime() : null;

    return (
      (this.filter.name === '' || b.customer.name.toLowerCase().includes(this.filter.name.toLowerCase())) &&
      (this.filter.email === '' || b.customer.email.toLowerCase().includes(this.filter.email.toLowerCase())) &&
      (this.filter.phone === '' || b.customer.phone.includes(this.filter.phone)) &&
      (this.filter.status === '' || b.status === this.filter.status) &&
      (!slotFrom || slot >= slotFrom) &&
      (!slotTo || slot <= slotTo) &&
      (!deliveryFrom || delivery >= deliveryFrom) &&
      (!deliveryTo || delivery <= deliveryTo)
    );
  });
}




}
