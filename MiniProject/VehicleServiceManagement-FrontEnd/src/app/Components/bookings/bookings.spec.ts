import { ComponentFixture, fakeAsync, TestBed, tick } from '@angular/core/testing';
import { Bookings } from './bookings';
import { BookingService } from '../../Services/BookingService';
import { UserService } from '../../Services/UserServices';
import { Store, provideStore } from '@ngrx/store';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { Booking } from '../../models/BookingsModel';
import { CommonModule } from '@angular/common';
import { provideHttpClient } from '@angular/common/http';
import { bookingReducer } from '../../Store/BookingStore/booking.reducer';
import { ElementRef } from '@angular/core';

describe('Bookings Component', () => {
  let component: Bookings;
  let fixture: ComponentFixture<Bookings>;
  let mockBookingService: jasmine.SpyObj<BookingService>;
  let mockUserService: jasmine.SpyObj<UserService>;
  let mockStore: jasmine.SpyObj<Store>;

  beforeEach(async () => {
    mockBookingService = jasmine.createSpyObj('BookingService', ['createBooking', 'getslots', 'cancelBooking']);
    mockUserService = jasmine.createSpyObj('UserService', ['getRole', 'getProfile', 'getId']);
    mockStore = jasmine.createSpyObj('Store', ['dispatch', 'select']);

    await TestBed.configureTestingModule({
      imports: [
        CommonModule,
        ReactiveFormsModule,
        Bookings
      ],
      providers: [
        { provide: BookingService, useValue: mockBookingService },
        { provide: UserService, useValue: mockUserService },
        { provide: Store, useValue: mockStore },
        provideHttpClient(),
        provideStore({ bookings: bookingReducer }) // optional if using real store
      ]
    }).compileComponents();

    mockUserService.getRole.and.returnValue('Customer');
    mockUserService.getProfile.and.returnValue(of({}));
    mockUserService.getId.and.returnValue('123');
    mockBookingService.getslots.and.returnValue(of([]));
    mockStore.select.and.returnValue(of([]));

    fixture = TestBed.createComponent(Bookings);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should select a slot and mark it touched', () => {
    component.selectSlot('2025-07-01T10:00');
    expect(component.slot?.value).toBe('2025-07-01T10:00');
    expect(component.slot?.touched).toBeTrue();
  });

  it('should handle image file input', () => {
    const file = new File(['dummy'], 'test.png', { type: 'image/png' });
    const event = {
      target: { files: [file] }
    } as unknown as Event;

    component.onImageSelected(event);
    expect(component.image?.value).toBe(file);
    expect(component.image?.touched).toBeTrue();
  });
it('should book a new service and show success message', fakeAsync(() => {
  mockBookingService.createBooking.and.returnValue(of({ success: true }));

  component.fileInput = {
    nativeElement: {
      value: ''
    }
  } as ElementRef;

  component.id = '123';
  component.bookingForm.setValue({
    phone: '9999999999',
    slot: '2025-07-01T10:00',
    image: new File([''], 'test.png')
  });

  component.book();
  tick(1000);  // Wait for any internal setTimeouts or debounce
  expect(component.showMessage).toBeTrue();
  expect(component.message).toContain('Successful');
  expect(mockBookingService.createBooking).toHaveBeenCalled();
}));


  it('should update form display when clicking show form', () => {
    component.isBooked = false;
    component.clickshowform();
    expect(component.showform).toBeTrue();
  });


it('should not allow booking if already booked', fakeAsync(() => {
  component.isBooked = true;
  
  component.clickshowform();
  tick(1000);

  expect(component.message).toBe('Already Booked a slot ');
  expect(component.showform).toBeFalse();
}));


  it('should handle booking cancellation', fakeAsync(() => {
    mockBookingService.cancelBooking.and.returnValue(of({}));
    component.bookings = [
      new Booking({ bookingID: 1, status: 'Booked', customer: { name: 'John' } })
    ];
    component.cancelBooking(1, 0);
    tick(1000);
    expect(component.bookings[0].status).toBe('Cancelled');
    expect(component.showCancelMessage).toBeTrue();
    expect(component.message).toContain('cancelled');
  }));

  it('should apply booking filters', () => {
    const now = new Date().toISOString();
    component.bookings = [
      new Booking({
        bookingID: 1,
        slot: now,
        deliveryTime: now,
        status: 'Booked',
        customer: { name: 'Test', email: 'test@mail.com', phone: '9999999999' }
      })
    ];
    component.filter.name = 'test';
    component.applyFilter();
    expect(component.filteredBookings.length).toBe(1);
  });
});
