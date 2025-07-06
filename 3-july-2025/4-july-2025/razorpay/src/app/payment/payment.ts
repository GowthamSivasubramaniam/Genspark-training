import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-payment',
  imports: [ReactiveFormsModule],
  templateUrl: './payment.html',
  styleUrl: './payment.css'
})
export class Payment {

  paymentForm:FormGroup


  paymentStatus:string = "";

 constructor()
 {
  this.paymentForm = new FormGroup({
    amount: new FormControl(null, [Validators.required, Validators.min(1)]),
    name: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    contact: new FormControl('', [
      Validators.required,
      Validators.pattern(/^\d{10}$/),
    ]),
  });
 }
  payNow() {
    if (this.paymentForm.invalid) {
      this.paymentForm.markAllAsTouched();
      return;
    }

    const { amount, name, email, contact } = this.paymentForm.value;

    const options: any = {
      key: 'rzp_test_1234567890', 
      amount: amount! * 100, // in paise
      currency: 'INR',
      name: 'Angular Payment',
      description: 'Demo Payment',
      order_id: 'order_dummy123456', 
      prefill: {
        name,
        email,
        contact,
        method: 'upi',
        upi: {
          vpa: 'success@razorpay', // simulated successful UPI
        },
      },
      handler: (response: any) => {
        this.paymentStatus = `✅ Payment Successful! ID: ${response.razorpay_payment_id}`;
      },
      modal: {
        ondismiss: () => {
          this.paymentStatus = '❌ Payment Cancelled or Failed.';
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


