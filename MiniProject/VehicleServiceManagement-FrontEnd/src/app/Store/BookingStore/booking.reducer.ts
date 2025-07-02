import { createReducer,on} from "@ngrx/store";
import * as actions from "./booking.actions";
import { IntialBookingState } from "./BookingState";



export const bookingReducer = createReducer(IntialBookingState,
      on(actions.loadBookings, (state) => ({
    ...state,
    loading: true,
    error: null
  })),

    on(actions.loadBookingsSuccess, (state,{bookings})=>({...state, bookings, loading: false, error: null })),
    on(actions.loadBookingsFailure, (state,{error})=>({...state, loading: false, error })),

)