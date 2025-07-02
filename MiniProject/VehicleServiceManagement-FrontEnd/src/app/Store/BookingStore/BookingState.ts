import { Booking } from "../../models/BookingsModel";


export interface BookingsState {
  bookings: Booking[];
  loading: boolean;
  error: string | null;
}

export const IntialBookingState: BookingsState = {
  bookings: [],
  loading: false,
  error: null
};
