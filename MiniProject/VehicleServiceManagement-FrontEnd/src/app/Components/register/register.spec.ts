import { ComponentFixture, TestBed, fakeAsync, tick } from '@angular/core/testing';
import { Register } from './register';
import { UserService } from '../../Services/UserServices';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

describe('Register Component', () => {
  let component: Register;
  let fixture: ComponentFixture<Register>;
  let userServiceSpy: jasmine.SpyObj<UserService>;

  beforeEach(async () => {
    userServiceSpy = jasmine.createSpyObj('UserService', ['registerUser']);

    await TestBed.configureTestingModule({
      imports: [Register, ReactiveFormsModule],
      providers: [{ provide: UserService, useValue: userServiceSpy }]
    }).compileComponents();

    fixture = TestBed.createComponent(Register);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the register component', () => {
    expect(component).toBeTruthy();
  });

  it('should have invalid form when empty', () => {
    expect(component.registerForm.valid).toBeFalse();
  });

  it('should validate form controls', () => {
    const name = component.Name;
    const email = component.Email;
    const phone = component.PhoneNumber;
    const password = component.Password;
    const confirmPassword = component.ConfirmPassword;

    name.setValue('JohnDoe');
    email.setValue('john@gmail.com');
    phone.setValue('9876543210');
    password.setValue('StrongPass1!');
    confirmPassword.setValue('StrongPass1!');

    expect(component.registerForm.valid).toBeTrue();
  });

  it('should emit form data and not call service if isChild is true', () => {
    component.isChild = true;
    spyOn(component.formSubmitted, 'emit');

    component.Name.setValue('Child User');
    component.Email.setValue('child@example.com');
    component.PhoneNumber.setValue('9876543210');
    component.Password.setValue('Pass1234!');
    component.ConfirmPassword.setValue('Pass1234!');

    component.registerUser();

    expect(component.formSubmitted.emit).toHaveBeenCalledWith({
      name: 'Child User',
      email: 'child@example.com',
      phone: '9876543210',
      password: 'Pass1234!'
    });
    expect(userServiceSpy.registerUser).not.toHaveBeenCalled();
  });

  it('should call service and show success message on successful registration', fakeAsync(() => {
    userServiceSpy.registerUser.and.returnValue(of({}));

    component.Name.setValue('User');
    component.Email.setValue('user@example.com');
    component.PhoneNumber.setValue('9876543210');
    component.Password.setValue('Pass1234!');
    component.ConfirmPassword.setValue('Pass1234!');

    component.registerUser();
    tick(1000);

    expect(userServiceSpy.registerUser).toHaveBeenCalledWith({
      name: 'User',
      email: 'user@example.com',
      phone: '9876543210',
      password: 'Pass1234!'
    });
    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Registration Successful');
  }));

  it('should show error message when registration fails', fakeAsync(() => {
    userServiceSpy.registerUser.and.returnValue(throwError(() => new Error('Error')));

    component.Name.setValue('User');
    component.Email.setValue('user@example.com');
    component.PhoneNumber.setValue('9876543210');
    component.Password.setValue('Pass1234!');
    component.ConfirmPassword.setValue('Pass1234!');

    component.registerUser();
    tick(1000);

    expect(component.showMessage).toBeTrue();
    expect(component.message).toBe('Registration failed. Please try again.');
  }));
});
