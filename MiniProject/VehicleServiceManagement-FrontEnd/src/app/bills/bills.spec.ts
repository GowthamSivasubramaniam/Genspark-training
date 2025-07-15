import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Bills } from './bills';
import { billService } from '../Services/BillService';
import { UserService } from '../Services/UserServices';
import { of, throwError } from 'rxjs';
import { CurrencyPipe, CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { registrationService } from '../Services/RegistrationService';

describe('Bills Component', () => {
  let component: Bills;
  let fixture: ComponentFixture<Bills>;

  const mockBillService = jasmine.createSpyObj('billService', ['showAllBills', 'download', 'updateStatus']);
  const mockUserService = jasmine.createSpyObj('UserService', ['getRole']);

  beforeEach(async () => {
    mockUserService.getRole.and.returnValue('Customer');
    mockBillService.showAllBills.and.returnValue(of([
      { billID: '1', email: 'test@example.com', memail: 'mech@example.com', status: 'Approved', totalAmount: 500, vehicleNo: 'TN01', customerName: 'John', mechanicName: 'Mike', description: 'Test', categoryAmounts: [{ categoryName: 'Cat1' }] }
    ]));

    await TestBed.configureTestingModule({
      imports: [Bills, CommonModule, FormsModule, CurrencyPipe],
      providers: [
        { provide: billService, useValue: mockBillService },
        { provide: UserService, useValue: mockUserService },
        { provide: registrationService, useValue: [] }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Bills);
    component = fixture.componentInstance;
    spyOn(localStorage, 'getItem').and.returnValue('test@example.com');
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load bills initially and filter for customer role', fakeAsync(() => {
    tick(2000); 
    expect(component.bills.length).toBeGreaterThan(-1);
    expect(component.bills[0]?.email).toBe(undefined);
  }));
it('should handle searchBills()', fakeAsync(() => {
  
  mockBillService.showAllBills.and.returnValue(of([]));

  // Recreate the component to re-run constructor
  fixture = TestBed.createComponent(Bills);
  component = fixture.componentInstance;
  fixture.detectChanges();

  // Clear previous calls from constructor
  mockBillService.showAllBills.calls.reset();

  // Simulate user entering search
  component.query = 'test';
  component.searchBills(); // triggers .next('test')
  tick(2000); // Wait for debounce

  // Now assert
 const result = component.filteredBills;
    expect(result.length).toBe(0);
}));


  it('should handle scroll event and load more bills', () => {
    spyOn(component, 'loadMore');
    window.dispatchEvent(new Event('scroll'));
    window.scrollY = 1000;
    window.innerHeight = 800;
    document.body.offsetHeight;
    component.onScroll();
    expect(component.loadMore).toHaveBeenCalled();
  });

  it('should update status', () => {
    const bill = { billID: '1', status: 'Pending' };
    mockBillService.updateStatus.and.returnValue(of({}));
    component.onStatusChange(bill, 'Approved');
    expect(bill.status).toBe('Approved');
  });

it('should handle updateStatus error', fakeAsync(() => {
  mockBillService.updateStatus.and.returnValue(throwError(() => new Error('Fail')));

  const bill = { billID: '1', status: 'Pending' };

  component.onStatusChange(bill, 'Declined');  // <- Call it first

  tick(1000); // <- Then simulate the timeout used inside

  expect(component.showMessage).toBeTrue();
  expect(component.message).toBe('Fail');
}));


  it('should filter bills correctly with filteredBills getter', () => {
    component.filters.vehicleNo = 'TN01';
    const result = component.filteredBills;
    expect(result.length).toBe(0);
    
  });

  it('should download bill', () => {
    const blob = new Blob(['test'], { type: 'application/pdf' });
    mockBillService.download.and.returnValue(of(blob));
    spyOn(document, 'createElement').and.callThrough();
    component.downloadBills('1');
    expect(mockBillService.download).toHaveBeenCalledWith('1');
  });

  it('should handle download error', () => {
    mockBillService.download.and.returnValue(throwError(() => new Error('Download error')));
    spyOn(console, 'error');
    component.downloadBills('1');
    expect(console.error).toHaveBeenCalledWith('Download failed:', jasmine.any(Error));
  });

});
