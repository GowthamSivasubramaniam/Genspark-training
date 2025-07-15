import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Login } from './login';
import { UserService } from '../../Services/UserServices';
import { Router } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

describe('Login Component', () => {
  let component: Login;
  let fixture: ComponentFixture<Login>;
  let mockUserService: jasmine.SpyObj<UserService>;
  let mockRouter: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    mockUserService = jasmine.createSpyObj('UserService', ['loginUser']);
    mockRouter = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
     
      imports: [ReactiveFormsModule,Login],
      providers: [
        { provide: UserService, useValue: mockUserService },
        { provide: Router, useValue: mockRouter }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Login);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the login component', () => {
    expect(component).toBeTruthy();
  });

  it('should have invalid form when empty', () => {
    expect(component.loginForm.valid).toBeFalse();
  });

  it('should return form controls Email and Password', () => {
    expect(component.Email).toBe(component.loginForm.get('Email'));
    expect(component.Password).toBe(component.loginForm.get('Password'));
  });

  it('should call loginUser and navigate on successful login', fakeAsync(() => {
    const mockResponse = { token: 'abc123', email: 'test@example.com' };
    mockUserService.loginUser.and.returnValue(of(mockResponse));

    component.loginForm.setValue({
      Email: 'test@example.com',
      Password: 'ValidPass123!'
    });

    component.loginUser();
    tick();

    expect(mockUserService.loginUser).toHaveBeenCalledWith({
      email: 'test@example.com',
      password: 'ValidPass123!'
    });

    expect(localStorage.getItem('token')).toBe(mockResponse.token);
    expect(localStorage.getItem('email')).toBe(mockResponse.email);
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/main']);
  }));

  it('should show error message on login failure', fakeAsync(() => {
    const errorResponse = { error: { message: 'Invalid credentials' } };
    mockUserService.loginUser.and.returnValue(throwError(() => errorResponse));

    component.loginForm.setValue({
      Email: 'wrong@example.com',
      Password: 'wrongpassword'
    });

    component.loginUser();
    tick();
     tick(1000);
    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Invalid credentials');
    expect(mockRouter.navigate).not.toHaveBeenCalled();
  }));
});
