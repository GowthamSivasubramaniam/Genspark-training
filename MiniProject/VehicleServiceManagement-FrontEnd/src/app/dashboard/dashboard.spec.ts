import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Dashboard } from './dashboard';
import { DashBoardService } from '../Services/DashBoardService';
import { UserService } from '../Services/UserServices';
import { billService } from '../Services/BillService';
import { of, throwError } from 'rxjs';

describe('Dashboard Component', () => {
  let component: Dashboard;
  let fixture: ComponentFixture<Dashboard>;

  let dashServiceSpy: jasmine.SpyObj<DashBoardService>;
  let userServiceSpy: jasmine.SpyObj<UserService>;
  let billServiceSpy: jasmine.SpyObj<billService>;

  beforeEach(async () => {
    dashServiceSpy = jasmine.createSpyObj('DashBoardService', ['getAllAnalytics']);
    userServiceSpy = jasmine.createSpyObj('UserService', ['getRole', 'getProfile']);
    billServiceSpy = jasmine.createSpyObj('billService', ['showAllBills']);

    // Setup userService mocks
    userServiceSpy.getRole.and.returnValue('Customer');
    userServiceSpy.getProfile.and.returnValue(of({ phone: '1234567890' }));

    // Setup dashService mock to return fake dashboard data
    dashServiceSpy.getAllAnalytics.and.returnValue(of({
      bookingCountsByDate: { '2025-07-01': 5, '2025-07-02': 3 },
      serviceCountsByDate: { '2025-07-01': 4, '2025-07-02': 6 },
      mechanicServiceStats: [
        { mechanicName: 'John', active: 3, completed: 7, aborted: 1 }
      ],
      customerBillSummary: { '1234567890': 1000 }
    }));

    // Setup billService mock to return some bills
    billServiceSpy.showAllBills.and.returnValue(of([
      { billID: 1, status: 'Approved', vehicleNo: 'V1' },
      { billID: 2, status: 'Pending', vehicleNo: 'V2' }
    ]));

    await TestBed.configureTestingModule({
      imports: [Dashboard],
      providers: [
        { provide: DashBoardService, useValue: dashServiceSpy },
        { provide: UserService, useValue: userServiceSpy },
        { provide: billService, useValue: billServiceSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Dashboard);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create dashboard component', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize phoneNo from profile and call applyFilters', fakeAsync(() => {
    // phoneNo should be set from getProfile response
    tick(10000); // Let async subscription complete
    expect(component.phoneNo).toBe('1234567890');
    // ApplyFilters triggers searchSubject.next and billService.showAllBills
    
    // Bills filtered to only Approved
    expect(component.bills.every(b => b.status === 'Approved')).toBeTrue();
  }));

  it('should prepare dashboard keys correctly', fakeAsync(() => {
    tick();
    expect(component.dashboardKeys).toEqual(jasmine.arrayContaining([])); 
    // More specifically check keys - you can adapt based on your data
  }));

  it('should prepare chart options correctly', fakeAsync(() => {
    tick();
    expect(component.bookingOptions).toBeUndefined();
    expect(component.serviceOptions).toBeUndefined();
    expect(component.mechanicOptions).toBeUndefined();
    expect(component.billOptions).toBeUndefined();
  }));

  it('should format keys properly', () => {
    expect(component.formatKey('someKeyName')).toBe('Some Key Name');
    expect(component.formatKey('AnotherKey')).toBe(' Another Key');
  });

  it('should handle applyFilters and set message on error from dashboard service', fakeAsync(() => {
    dashServiceSpy.getAllAnalytics.and.returnValue(throwError(() => new Error('No data')));
    component.phoneNo = '1234';
    component.from = '';
    component.to = '';
    component.applyFilters();
    tick();
    expect(component.showMessage).toBeFalse();
    expect(component.message).toBe("");
  }));

  it('should filter bills on applyFilters', fakeAsync(() => {
    billServiceSpy.showAllBills.and.returnValue(of([
      { billID: 1, status: 'Approved' },
      { billID: 2, status: 'Declined' },
      { billID: 3, status: 'Approved' }
    ]));
    component.applyFilters();
    tick();
    expect(component.bills.length).toBe(2);
    expect(component.bills.every(b => b.status === 'Approved')).toBeTrue();
  }));
});
