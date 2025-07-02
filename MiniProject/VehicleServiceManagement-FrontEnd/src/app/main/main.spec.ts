import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Main } from './main';
import { UserService } from '../Services/UserServices';
import { ActivatedRoute, Router } from '@angular/router';
import { Store } from '@ngrx/store';
import * as BookingActions from '../Store/BookingStore/booking.actions';
import { of } from 'rxjs';

describe('Main Component', () => {
  let component: Main;
  let fixture: ComponentFixture<Main>;
  let userServiceSpy: jasmine.SpyObj<UserService>;
  let routerSpy: jasmine.SpyObj<Router>;
  let storeSpy: jasmine.SpyObj<Store>;
  const activatedRouteMock = {
    snapshot: { params: {}, queryParams: {} },
    params: of({}),
    queryParams: of({})
  };
  beforeEach(async () => {
    userServiceSpy = jasmine.createSpyObj('UserService', ['getRole', 'logout']);
    routerSpy = jasmine.createSpyObj('Router', ['navigate']);
    storeSpy = jasmine.createSpyObj('Store', ['dispatch']);

    userServiceSpy.getRole.and.returnValue('Admin');

    await TestBed.configureTestingModule({
      imports: [Main],
      providers: [
        { provide: UserService, useValue: userServiceSpy },
        { provide: Router, useValue: routerSpy },
        { provide: Store, useValue: storeSpy },
        {provide: ActivatedRoute,useValue: activatedRouteMock }
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(Main);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create and set role', () => {
    expect(component).toBeTruthy();
    expect(component.role).toBe('Admin');
  });

  it('should dispatch loadBookings action on init', () => {
    expect(storeSpy.dispatch).toHaveBeenCalledWith(BookingActions.loadBookings());
  });

  it('should call logout and navigate on logout()', () => {
    component.logout();
    expect(userServiceSpy.logout).toHaveBeenCalled();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/home']);
  });
});
