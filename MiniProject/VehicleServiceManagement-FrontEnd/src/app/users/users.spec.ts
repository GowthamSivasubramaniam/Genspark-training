import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Users } from './users';
import { UserService } from '../Services/UserServices';
import { of, throwError } from 'rxjs';
import { Register } from '../Components/register/register';
import { FormsModule } from '@angular/forms';

describe('Users Component', () => {
  let component: Users;
  let fixture: ComponentFixture<Users>;
  let userServiceSpy: jasmine.SpyObj<UserService>;

  beforeEach(async () => {
    const Userspy = jasmine.createSpyObj('UserService', [
      'getAllCustomers',
      'getAllMechanics',
      'registerMechanic',
      'deactivateProfile'
    ]);
  Userspy.getAllCustomers.and.returnValue(of([
    { name: 'Customer1', email: 'c1@test.com', phone: '123', status: 'Active' }
  ]));
  Userspy.getAllMechanics.and.returnValue(of([
    { name: 'Mechanic1', email: 'm1@test.com', phone: '456', status: 'Active' }
  ]));

    await TestBed.configureTestingModule({
      imports: [Users, Register, FormsModule],
      providers: [{ provide: UserService, useValue: Userspy }]
    }).compileComponents();

    fixture = TestBed.createComponent(Users);
    component = fixture.componentInstance;
    userServiceSpy = TestBed.inject(UserService) as jasmine.SpyObj<UserService>;

    // Setup default spy returns for initial data fetch
    userServiceSpy.getAllCustomers.and.returnValue(of([
      { name: 'Customer1', email: 'c1@test.com', phone: '123', status: 'Active' }
    ]));
    userServiceSpy.getAllMechanics.and.returnValue(of([
      { name: 'Mechanic1', email: 'm1@test.com', phone: '456', status: 'Active' }
    ]));

    fixture.detectChanges(); // triggers ngOnInit & subscriptions
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load customer and mechanic data on init', () => {
    expect(userServiceSpy.getAllCustomers).toHaveBeenCalled();
    expect(userServiceSpy.getAllMechanics).toHaveBeenCalled();

    expect(component.CustomerData.length).toBe(1);
    expect(component.MechanicData.length).toBe(1);
  });

  it('should toggle showForm when clickshowform is called', () => {
    expect(component.showForm).toBeFalse();
    component.clickshowform();
    expect(component.showForm).toBeTrue();
  });

  it('should set showMech true if selectedRole is Mechanic and false otherwise', () => {
    component.selectedRole = 'Mechanic';
    component.changeView();
    expect(component.showMech).toBeTrue();

    component.selectedRole = 'Customer';
    component.changeView();
    expect(component.showMech).toBeFalse();
  });

  describe('registerMechanic', () => {
    it('should show success message on successful registration', fakeAsync(() => {
      userServiceSpy.registerMechanic.and.returnValue(of({}));
      component.registerMechanic({ name: 'NewMech' });
      tick(1000);
      expect(userServiceSpy.registerMechanic).toHaveBeenCalled();
      expect(component.showMessage).toBeTrue();
      expect(component.message).toBe('Registration successfull');
    }));

    it('should show failure message on registration error', fakeAsync(() => {
      userServiceSpy.registerMechanic.and.returnValue(throwError(() => new Error('fail')));
      component.registerMechanic({ name: 'NewMech' });
      tick(1000);
      expect(component.showMessage).toBeTrue();
      expect(component.message).toBe('Registration Failed');
    }));
  });

  describe('deactivateMechanic', () => {
    it('should update status and show success message on successful deactivation', fakeAsync(() => {
      const mech = { email: 'mech@test.com', status: 'Active' };
      userServiceSpy.deactivateProfile.and.returnValue(of({ status: 'Inactive' }));
      component.deactivateMechanic(mech);
      tick(1000)
      expect(userServiceSpy.deactivateProfile).toHaveBeenCalledWith(mech, mech.email);
      expect(mech.status).toBe('Inactive');
      expect(component.showMessage).toBeTrue();
      expect(component.message).toBe('Updation successfull');
    }));

    it('should show failure message on deactivation error', fakeAsync(() => {
      const mech = { email: 'mech@test.com', status: 'Active' };
      userServiceSpy.deactivateProfile.and.returnValue(throwError(() => new Error('fail')));
      component.deactivateMechanic(mech);
       tick(1000)
      expect(component.showMessage).toBeTrue();
      expect(component.message).toBe('Updation failed');
    }));
  });

  describe('filteredMechanics getter', () => {
    beforeEach(() => {
      component.MechanicData = [
        { name: 'Alice', email: 'a@test.com', phone: '111', status: 'Active' },
        { name: 'Bob', email: 'b@test.com', phone: '222', status: 'Inactive' }
      ];
    });

    it('should filter by name', () => {
      component.filter.name = 'ali';
      const filtered = component.filteredMechanics;
      expect(filtered.length).toBe(1);
      expect(filtered[0].name).toBe('Alice');
    });

    it('should filter by email', () => {
      component.filter.email = 'b@test.com';
      const filtered = component.filteredMechanics;
      expect(filtered.length).toBe(1);
      expect(filtered[0].email).toBe('b@test.com');
    });

    it('should filter by phone', () => {
      component.filter.phone = '222';
      const filtered = component.filteredMechanics;
      expect(filtered.length).toBe(1);
      expect(filtered[0].phone).toBe('222');
    });

    it('should filter by status', () => {
      component.filter.status = 'Active';
      const filtered = component.filteredMechanics;
      expect(filtered.length).toBe(1);
      expect(filtered[0].status).toBe('Active');
    });

    it('should return all when no filters applied', () => {
      component.filter = { name: '', email: '', phone: '', status: '' };
      const filtered = component.filteredMechanics;
      expect(filtered.length).toBe(2);
    });
  });

  describe('filteredCustomers getter', () => {
    beforeEach(() => {
      component.CustomerData = [
        { name: 'Charlie', email: 'c@test.com', phone: '333', status: 'Active' },
        { name: 'Dave', email: 'd@test.com', phone: '444', status: 'Inactive' }
      ];
    });

    it('should filter by name', () => {
      component.filter.name = 'char';
      const filtered = component.filteredCustomers;
      expect(filtered.length).toBe(1);
      expect(filtered[0].name).toBe('Charlie');
    });

    it('should filter by email', () => {
      component.filter.email = 'd@test.com';
      const filtered = component.filteredCustomers;
      expect(filtered.length).toBe(1);
      expect(filtered[0].email).toBe('d@test.com');
    });

    it('should filter by phone', () => {
      component.filter.phone = '444';
      const filtered = component.filteredCustomers;
      expect(filtered.length).toBe(1);
      expect(filtered[0].phone).toBe('444');
    });

    it('should filter by status', () => {
      component.filter.status = 'Active';
      const filtered = component.filteredCustomers;
      expect(filtered.length).toBe(1);
      expect(filtered[0].status).toBe('Active');
    });

    it('should return all when no filters applied', () => {
      component.filter = { name: '', email: '', phone: '', status: '' };
      const filtered = component.filteredCustomers;
      expect(filtered.length).toBe(2);
    });
  });
});
