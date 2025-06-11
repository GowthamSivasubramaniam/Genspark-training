import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './customer.html',
  styleUrls: ['./customer.css']
})
export class Customer {
  customer: { name: string; address: string; phno: string; likes: number; dislikes: number }[] = [];

  newCustomer = {
    name: '',
    address: '',
    phno: '',
    likes: 0,
    dislikes: 0
  };

  addCustomer() {
    this.customer.push({ ...this.newCustomer });
    this.newCustomer = { name: '', address: '', phno: '', likes: 0, dislikes: 0 }; 
  }

  like(index: number) {
    this.customer[index].likes += 1;
  }

  dislike(index: number) {
    this.customer[index].dislikes += 1;
  }
}
