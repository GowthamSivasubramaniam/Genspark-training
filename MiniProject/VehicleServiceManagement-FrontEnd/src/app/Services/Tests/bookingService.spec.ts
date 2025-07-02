import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { BookingService } from '../BookingService';
import { HttpHeaders } from '@angular/common/http';
import { Booking } from '../../models/BookingsModel';

describe('BookingService', () => {
  let service: BookingService;
  let httpMock: HttpTestingController;

  const token = 'test-token';
  const headers = new HttpHeaders({
    'Authorization': `Bearer ${token}`,
    'Content-Type': 'application/json'
  });
 const mockBooking: Booking = new Booking({
  bookingID: 'B123',
  customerID: 'C001',
  bookedAt: '2025-06-25T10:00:00Z',
  slot: '2025-06-30T15:00:00Z',
  deliveryTime: '2025-07-01T15:00:00Z',
  status: 'Booked',
  imageurl: 'https://example.com/image.jpg',
  customer: {
    customerID: 'C001',
    name: 'John Doe',
    phone: '9876543210',
    email: 'john@example.com',
    status: 'Active'
  }
});
  beforeEach(() => {
    localStorage.setItem('token', token);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [BookingService]
    });

    service = TestBed.inject(BookingService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
    localStorage.clear();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should create a booking', () => {
    const formData = new FormData();
    service.createBooking(formData).subscribe();
    const req = httpMock.expectOne('https://localhost:7176/api/v1/Booking');
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
    req.flush({});
  });

  it('should fetch all bookings', () => {
    const dummyBookings: Booking[] = [mockBooking];
    service.getallBooking().subscribe(data => {
      expect(data.length).toBe(1);
      expect(data).toEqual(dummyBookings);
    });

    const req = httpMock.expectOne('https://localhost:7176/api/v1/Booking');
    expect(req.request.method).toBe('GET');
    req.flush(dummyBookings);
  });

  it('should get all active bookings', () => {
    const dummyBookings: Booking[] = [mockBooking];
    service.getallActiveBookings().subscribe(data => {
      expect(data).toEqual(dummyBookings);
    });

    const req = httpMock.expectOne('https://localhost:7176/api/v1/Booking/Booked');
    expect(req.request.method).toBe('GET');
    req.flush(dummyBookings);
  });

  it('should fetch slots', () => {
    const dummySlots = ['9:00', '10:00'];
    service.getslots().subscribe(data => {
      expect(data).toEqual(dummySlots);
    });

    const req = httpMock.expectOne('https://localhost:7176/api/v1/Booking/slots');
    expect(req.request.method).toBe('GET');
    req.flush(dummySlots);
  });

  it('should cancel booking', () => {
    const bookingId = 123;
    service.cancelBooking(bookingId).subscribe();
    const req = httpMock.expectOne(`https://localhost:7176/Cancel/${bookingId}`);
    expect(req.request.method).toBe('PUT');
    req.flush({});
  });

  it('should update booking', () => {
    const bookingId = 456;
    service.UpdateBooking(bookingId).subscribe();
    const req = httpMock.expectOne(`https://localhost:7176/api/v1/Booking/${bookingId}`);
    expect(req.request.method).toBe('PUT');
    req.flush({});
  });
});
