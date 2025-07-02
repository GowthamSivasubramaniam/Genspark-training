import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Registrations } from './registrations';
import { registrationService } from '../Services/RegistrationService';
import { CategoryService } from '../Services/CategoryService';
import { BookingService } from '../Services/BookingService';
import { UserService } from '../Services/UserServices';
import { billService } from '../Services/BillService';
import { of, throwError } from 'rxjs';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

describe('Registrations Component', () => {
  let component: Registrations;
  let fixture: ComponentFixture<Registrations>;

  const mockRegistrationService = jasmine.createSpyObj('registrationService', [
    'showVehicles', 'showService', 'addVehicle', 'addService', 'addServiceRecord', 'updateStatus'
  ]);
  const mockCategoryService = jasmine.createSpyObj('CategoryService', ['getAllCategories']);
  const mockBookingService = jasmine.createSpyObj('BookingService', ['getallActiveBookings', 'UpdateBooking']);
  const mockUserService = jasmine.createSpyObj('UserService', ['getRole', 'getProfile', 'getId']);
  const mockBillService = jasmine.createSpyObj('billService', ['addBill']);

  const bookingMock = [{
    bookingID: 'b1',
    customer: { phone: '9999999999' },
    customerID: 'c1',
    bookedAt: new Date(),
    slot: '2025-07-01T10',
    deliveryTime: new Date(),
    status: 'Active',
    imageurl: 'someurl.jpg'
  }];

  beforeEach(async () => {
    mockCategoryService.getAllCategories.and.returnValue(of([{ name: 'Cat1', status: 'Active' }]));

    await TestBed.configureTestingModule({
      imports: [ReactiveFormsModule, FormsModule, CommonModule, Registrations],
      providers: [
        { provide: registrationService, useValue: mockRegistrationService },
        { provide: CategoryService, useValue: mockCategoryService },
        { provide: BookingService, useValue: mockBookingService },
        { provide: UserService, useValue: mockUserService },
        { provide: billService, useValue: mockBillService },
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Registrations);
    component = fixture.componentInstance;

    mockUserService.getRole.and.returnValue('Customer');
    mockBookingService.getallActiveBookings.and.returnValue(of(bookingMock));
    mockRegistrationService.showVehicles.and.returnValue(of([]));
    mockRegistrationService.showService.and.returnValue(of([]));
    mockUserService.getId.and.returnValue('user1');
    mockUserService.getProfile.and.returnValue(of({}));
    mockBookingService.UpdateBooking.and.returnValue(of({}));
    mockRegistrationService.addVehicle.and.returnValue(of({ vehicleNo: 'V123' }));
    mockRegistrationService.addService.and.returnValue(of({ serviceID: 's1' }));
    mockRegistrationService.addServiceRecord.and.returnValue(of({}));
    mockRegistrationService.updateStatus.and.returnValue(of({}));
    mockBillService.addBill.and.returnValue(of({}));

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load initial categories', () => {
    expect(component.allcategories.length).toBeGreaterThan(0);
  });

  it('should search vehicles and update vehicles array', fakeAsync(() => {
    const vehicles = [{ vehicleNo: 'V1' }];
    mockRegistrationService.showVehicles.and.returnValue(of(vehicles));
    component.vehicleQuery = 'V1';
    component.searchVehicles();
    tick(2000);
    expect(component.vehicles).toEqual([]);
  }));

  it('should add a vehicle successfully', () => {
    const vehicleData = { vehicleNo: 'V123', vehicleType: 'Car', vechicleManufacturer: 'Honda', vehicleModel: 'City' };
    mockRegistrationService.addVehicle.and.returnValue(of(vehicleData));
    component.vehicleAddForm.setValue({
      No: 'V123',
      Type: 'Car',
      Manufacturer: 'Honda',
      Model: 'City'
    });
    component.addVehicle();
    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Vehicle added Successfully');
    expect(component.vehicles).toContain(vehicleData);
  });

  it('should handle addVehicle error', () => {
    mockRegistrationService.addVehicle.and.returnValue(throwError(() => new Error('Error')));
    component.vehicleAddForm.setValue({
      No: 'V123',
      Type: 'Car',
      Manufacturer: 'Honda',
      Model: 'City'
    });
    component.addVehicle();
    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Vehicle cannot be added');
  });

  it('should handle addService success and update bookings', fakeAsync(() => {
    const serviceData = { serviceID: 's1' };
    const vehicleData = [{ vehicleID: 'v1' }];
    mockBookingService.getallActiveBookings.and.returnValue(of(bookingMock));
    mockRegistrationService.showVehicles.and.returnValue(of(vehicleData));
    mockRegistrationService.addService.and.returnValue(of(serviceData));
    mockRegistrationService.addServiceRecord.and.returnValue(of({}));
   
    component.ServiceAddForm.patchValue({
      VehicleNo: 'V123',
      Description: 'Test Service',
      Customer_Phno: '9999999999',
      Categories: ['Cat1']
    });

    component.addService();
    tick();
    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Added Successfully');
  }));

  it('should show bill form when status changed to Completed', () => {
    component.onStatusChange({ serviceRecordID: 'sr1', status: 'Active' }, 'Completed', 'sr1');
    expect(component.showBillForm).toBeTrue();
  });

  it('should update status when onStatusChange with status not Completed', fakeAsync(() => {
    const item = { serviceRecordID: 'sr2', status: 'Active' };
    (component as any).service.updateStatus = jasmine.createSpy().and.returnValue(of({}));
    component.onStatusChange(item, 'Aborted', item.serviceRecordID);
    tick();
    expect(item.status).toBe('Aborted');
  }));

  it('should filter vehicles correctly', () => {
    component.vehicles = [
      { vehicleType: 'Car', vechicleManufacturer: 'Honda', vehicleModel: 'City' },
      { vehicleType: 'Bike', vechicleManufacturer: 'Yamaha', vehicleModel: 'R15' }
    ];
    component.filters.vehicleType = 'Car';
    const filtered = component.filteredVehicles;
    expect(filtered.length).toBe(1);
    expect(filtered[0].vehicleType).toBe('Car');
  });

  it('should filter services correctly', () => {
    component.services = [
      {
        vehicleNo: 'V1', customerName: 'John', customer_Email: 'john@example.com',
        mechanicName: 'Mike', mechanic_Email: 'mike@example.com', status: 'Active', categories: ['Cat1']
      },
      {
        vehicleNo: 'V2', customerName: 'Anna', customer_Email: 'anna@example.com',
        mechanicName: 'Sara', mechanic_Email: 'sara@example.com', status: 'Completed', categories: ['Cat2']
      }
    ];
    component.filter = {
      vehicleNo: 'V1',
      customerName: '',
      email: '',
      mechanicName: '',
      mechanicEmail: '',
      category: 'Cat1',
      status: 'Active'
    };
    const filtered = component.filteredRecords;
    expect(filtered.length).toBe(1);
    expect(filtered[0].vehicleNo).toBe('V1');
  });
});
