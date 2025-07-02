import { createAction, props } from "@ngrx/store";
import { Booking } from "../../models/BookingsModel";



export const loadBookings = createAction('[Booking] Load');
export const loadBookingsSuccess = createAction('[Booking] Load Success', props<{ bookings: Booking[] }>());
export const loadBookingsFailure = createAction('[Booking] Load Failure', props<{ error: string }>());
