import { ApplicationConfig, provideBrowserGlobalErrorListeners, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { UserService } from './Services/UserServices';
import { provideState, provideStore } from '@ngrx/store';
import { bookingReducer } from './Store/BookingStore/booking.reducer';
import { provideEffects } from '@ngrx/effects';
import { BookingEffects } from './Store/BookingStore/booking.effect';
import { BookingService } from './Services/BookingService';
import { CategoryService } from './Services/CategoryService';
import { registrationService } from './Services/RegistrationService';
import { billService } from './Services/BillService';
import { DashBoardService } from './Services/DashBoardService';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideHttpClient(),
    provideRouter(routes),
    provideStore(),
    provideState('bookings',bookingReducer ),
    provideEffects(BookingEffects),
  BookingService,
  UserService,
  CategoryService,
  registrationService,
  billService,
  DashBoardService
  ]
};
