import { TestBed } from '@angular/core/testing';
import { registrationService } from '../RegistrationService';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('registrationService', () => {
  let service: registrationService;
  let httpMock: HttpTestingController;
  const token = 'test-token';

  beforeEach(() => {
    localStorage.setItem('token', token);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [registrationService]
    });
    service = TestBed.inject(registrationService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    localStorage.clear();
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add vehicle', () => {
    const vehicle = { name: 'Test Car' };
    service.addVehicle(vehicle).subscribe();
    const req = httpMock.expectOne('https://localhost:7176/api/v1/Vehicle');
    expect(req.request.method).toBe('POST');
    expect(req.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
    req.flush({});
  });

  it('should show vehicles with query', () => {
    const query = 'test';
    service.showVehicles(query, 1, 10).subscribe();
    const req = httpMock.expectOne(`https://localhost:7176/api/v1/Vehicle?page=1&pageSize=10&search=test`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should show vehicles without query', () => {
    service.showVehicles('', 2, 5).subscribe();
    const req = httpMock.expectOne(`https://localhost:7176/api/v1/Vehicle?page=2&pageSize=5`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should show service records with query', () => {
    service.showService('abc', 1, 5).subscribe();
    const req = httpMock.expectOne(`https://localhost:7176/api/v1/ServiceRecord?page=1&pageSize=5&search=abc`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should show service records without query', () => {
    service.showService('', 2, 10).subscribe();
    const req = httpMock.expectOne(`https://localhost:7176/api/v1/ServiceRecord?page=2&pageSize=10`);
    expect(req.request.method).toBe('GET');
    req.flush([]);
  });

  it('should add service', () => {
    const data = { name: 'Oil Change' };
    service.addService(data).subscribe();
    const req = httpMock.expectOne('https://localhost:7176/api/v1/Service');
    expect(req.request.method).toBe('POST');
    req.flush({});
  });

  it('should add service record', () => {
    const data = { serviceId: 1 };
    service.addServiceRecord(data).subscribe();
    const req = httpMock.expectOne('https://localhost:7176/api/v1/ServiceRecord');
    expect(req.request.method).toBe('POST');
    req.flush({});
  });

  it('should handle error in addServiceRecord', () => {
    const data = { serviceId: 1 };
    service.addServiceRecord(data).subscribe({
      error: (err:any) => {
        expect(err.status).toBe(500);
      }
    });
    const req = httpMock.expectOne('https://localhost:7176/api/v1/ServiceRecord');
    req.flush('Server error', { status: 500, statusText: 'Internal Server Error' });
  });

  it('should update status', () => {
    service.updateStatus(1, 'Approved').subscribe();
    const req = httpMock.expectOne('https://localhost:7176/api/v1/ServiceRecord/status');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual({ serviceRecordID: 1, status: 'Approved' });
    req.flush({});
  });

  it('should handle error on update status', () => {
    service.updateStatus(1, 'Rejected').subscribe({
      error: (err) => {
        expect(err.status).toBe(400);
      }
    });
    const req = httpMock.expectOne('https://localhost:7176/api/v1/ServiceRecord/status');
    req.flush('Bad request', { status: 400, statusText: 'Bad Request' });
  });
});
