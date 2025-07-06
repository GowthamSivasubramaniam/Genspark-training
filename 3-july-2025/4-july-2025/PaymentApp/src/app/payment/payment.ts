import { Component } from '@angular/core';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

@Component({
  selector: 'app-payment',
  standalone: true,
  imports: [ReactiveFormsModule],
  templateUrl: './payment.html',
  styleUrl: './payment.css',
})
export class Payment {
  paymentForm: FormGroup;
  paymentStatus: string = '';
  history: any[] = [];
  showsuccess = false
  constructor() {
    this.paymentForm = new FormGroup({
      amount: new FormControl(null, [Validators.required, Validators.min(1)]),
      name: new FormControl('', Validators.required),
      email: new FormControl('', [Validators.required, Validators.email]),
      contact: new FormControl('', [
        Validators.required,
        Validators.pattern(/^\d{10}$/),
      ]),
    });

    const stored = localStorage.getItem('Payments');
    this.history = stored ? JSON.parse(stored) : [];
    console.log(this.history);
  }

  payNow() {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      return;
    }

    const { amount, name, email, contact } = this.paymentForm.value;

    const options: any = {
      key: 'rzp_test_yHkufqkdPQK6Hl',
      amount: amount! * 100,
      currency: 'INR',
      name: name,
      description: 'Demo Payment',
      prefill: {
        name,
        email,
        contact,
        method: 'upi',
        upi: {
          vpa: 'success@razorpay',
        },
      },
      handler: (response: any) => {
        this.paymentStatus = `Payment Successful!`;
        this.showsuccess = true
        const newPayment = { amount, name, email, contact, staus: "success" };
        const existing = localStorage.getItem('Payments');
        const payments = existing ? JSON.parse(existing) : [];

        payments.push(newPayment);
        localStorage.setItem('Payments', JSON.stringify(payments));

        this.history = payments;
        setTimeout(() => {
          this.showsuccess = false;
          this.paymentStatus = '';
        }, 4000);
        this.paymentForm.reset();
      },
      modal: {
        ondismiss: () => {
          this.paymentStatus = 'Payment Failed.';
          this.showsuccess = true
          const newPayment = { amount, name, email, contact, staus: "failed" };
          const existing = localStorage.getItem('Payments');
          const payments = existing ? JSON.parse(existing) : [];
          payments.push(newPayment);
          localStorage.setItem('Payments', JSON.stringify(payments));
          this.history = payments;
          setTimeout(() => {
            this.showsuccess = false;
            this.paymentStatus = '';
          }, 4000);
          this.paymentForm.reset();
        },
      },
      theme: {
        color: '#3399cc',
      },
    };

    const rzp = new (window as any).Razorpay(options);
    rzp.open();
  }
}
