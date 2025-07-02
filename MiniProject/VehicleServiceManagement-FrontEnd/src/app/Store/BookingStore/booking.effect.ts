import { Injectable } from '@angular/core';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import * as BookingActions from './booking.actions';
import { catchError, map, switchMap, of, tap } from 'rxjs';
import { BookingService } from '../../Services/BookingService';

@Injectable()
export class BookingEffects {
  loadBookings$;

  constructor(private actions$: Actions, private bookingService: BookingService) {

    console.log("fioioi")
    this.loadBookings$ = createEffect(() => 
      this.actions$.pipe(
        ofType(BookingActions.loadBookings),
          switchMap(() =>
          this.bookingService.getallBooking().pipe(
            map(bookings => BookingActions.loadBookingsSuccess({ bookings })),
            catchError(error => {
              console.error('[BookingEffects] Error loading bookings:', error);
              return of(BookingActions.loadBookingsFailure({ error }));
            })
          )
        )
      )
  );

  }
}
