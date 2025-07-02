import { HttpClient, HttpHeaders } from "@angular/common/http";
import { inject, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Booking } from "../models/BookingsModel";

@Injectable({
  providedIn: 'root',
})
export class BookingService {
    private http = inject(HttpClient);
    createBooking(booking: FormData) {
        return this.http.post('https://localhost:7176/api/v1/Booking', booking,{
    headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
    })
  });
    }
    getallBooking(): Observable<Booking[]> {
        console.log("hi")
        return this.http.get<Booking[]>('https://localhost:7176/api/v1/Booking',{
         headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    })
  });
    }
    getslots()
    {
         return this.http.get('https://localhost:7176/api/v1/Booking/slots',{
    headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    })
  });
   
    }

    cancelBooking(id:any)
   {
      return this.http.put(`https://localhost:7176/Cancel/${id}`,null,{
    headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    })
  })
}
    UpdateBooking(id:any)
   {
      return this.http.put(`https://localhost:7176/api/v1/Booking/${id}`,null,{
    headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    })
  })
   }

    getallActiveBookings()  {
       
        return this.http.get<Booking[]>('https://localhost:7176/api/v1/Booking/Booked',{
         headers: new HttpHeaders({
      'Authorization': `Bearer ${localStorage.getItem('token')}`,
      'Content-Type': 'application/json'
    })
  });
    }
}



