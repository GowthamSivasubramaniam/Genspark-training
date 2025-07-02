import { Component } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { UserService } from '../Services/UserServices';
import { Store } from '@ngrx/store';
import * as BookingActions from '../Store/BookingStore/booking.actions';

@Component({
  selector: 'app-main',
  imports: [RouterOutlet,RouterLink],
  templateUrl: './main.html',
  styleUrl: './main.css'
})
export class Main {
    role:any
    
    
   constructor(private service:UserService,private route:Router, private store: Store) {
     this.store.dispatch(BookingActions.loadBookings());
    this.role =service.getRole()
  }
  
  logout()
  {
    this.service.logout()
    this.route.navigate(["/home"])
  }
  
}
