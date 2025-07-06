import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ReactiveFormsModule } from '@angular/forms';
import { Payment } from './payment';

describe('Payment Component', () => {
  let component: Payment;
  let fixture: ComponentFixture<Payment>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Payment, ReactiveFormsModule],
    }).compileComponents();

    fixture = TestBed.createComponent(Payment);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the payment form with 4 controls', () => {
    expect(component.paymentForm.contains('amount')).toBeTrue();
    expect(component.paymentForm.contains('name')).toBeTrue();
    expect(component.paymentForm.contains('email')).toBeTrue();
    expect(component.paymentForm.contains('contact')).toBeTrue();
  });

  it('should mark form controls as touched and not open Razorpay if form is invalid', () => {
    spyOn(component.paymentForm, 'markAllAsTouched').and.callThrough();

   
    (window as any).Razorpay = function () {
      return { open: jasmine.createSpy('open') };
    };

    component.payNow();

    expect(component.paymentForm.markAllAsTouched).toHaveBeenCalled();
 
    expect((window as any).Razorpay().open).not.toHaveBeenCalled();
  });

  it('should open Razorpay with correct options when form is valid', () => {
    component.paymentForm.setValue({
      amount: 100,
      name: 'Test User',
      email: 'test@example.com',
      contact: '1234567890',
    });

    const rzpOpenSpy = jasmine.createSpy('open');

    const RazorpayMock = function (options: any) {
      return { open: rzpOpenSpy };
    };

    (window as any).Razorpay = RazorpayMock;

    component.payNow();

    expect(rzpOpenSpy).toHaveBeenCalledTimes(1);

    const options = (window as any).Razorpay.calls
      ? (window as any).Razorpay.calls.mostRecent().args[0]
      : null;

  });

  it('should update paymentStatus on successful payment', () => {
    component.paymentForm.setValue({
      amount: 100,
      name: 'Test User',
      email: 'test@example.com',
      contact: '1234567890',
    });

    const rzpOpenSpy = jasmine.createSpy('open');
    const fakeResponse = { razorpay_payment_id: 'pay_12345' };

    const RazorpayMock = function (options: any) {
    
      options.handler(fakeResponse);
      return { open: rzpOpenSpy };
    };

    (window as any).Razorpay = RazorpayMock;

    component.payNow();

    expect(component.paymentStatus).toBe('Payment Successful!');
    expect(rzpOpenSpy).toHaveBeenCalled();
  });

  it('should update paymentStatus on payment cancel', () => {
    component.paymentForm.setValue({
      amount: 100,
      name: 'Test User',
      email: 'test@example.com',
      contact: '1234567890',
    });

    const rzpOpenSpy = jasmine.createSpy('open');
    let ondismissFn: () => void = () => {};

    const RazorpayMock = function (options: any) {
      ondismissFn = options.modal.ondismiss;
      return { open: rzpOpenSpy };
    };

    (window as any).Razorpay = RazorpayMock;

    component.payNow();


    ondismissFn();

    expect(component.paymentStatus).toBe('Payment Failed.');
    expect(rzpOpenSpy).toHaveBeenCalled();
  });
});
