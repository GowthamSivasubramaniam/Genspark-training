import { createFeatureSelector, createSelector } from "@ngrx/store";
import { BookingsState } from "./BookingState";


export const selectBookingState = createFeatureSelector<BookingsState>('bookings');


export const selectAllBookings = createSelector(
  selectBookingState,
  (state) => state.bookings
);

export const selectBookingLoading = createSelector(
  selectBookingState,
  (state) => state.loading
);

export const selectBookingError = createSelector(
  selectBookingState,
  (state) => state.error
);
