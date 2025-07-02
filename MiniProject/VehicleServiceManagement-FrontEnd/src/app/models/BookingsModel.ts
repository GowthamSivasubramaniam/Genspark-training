export class Customer {
  customerID: string;
  name: string;
  phone: string;
  email: string;
  status: string;

  constructor(data?: Partial<Customer>) {
    this.customerID = data?.customerID ?? '';
    this.name = data?.name ?? '';
    this.phone = data?.phone ?? '';
    this.email = data?.email ?? '';
    this.status = data?.status ?? '';
  }
}

export class Booking {
  bookingID: string;
  customerID: string;
  bookedAt: Date;
  slot: string;
  deliveryTime: Date;
  status: string;
  imageurl: string;
  customer: Customer;
   
  constructor(data?: any) {
    this.bookingID = data?.bookingID ?? '';
    this.customerID = data?.customerID ?? '';
    this.bookedAt = data?.bookedAt ? new Date(data.bookedAt) : new Date();
    this.slot = data?.slot ?? '';
    this.deliveryTime = data?.deliveryTime ? new Date(data.deliveryTime) : new Date();
    this.status = data?.status ?? '';
    this.imageurl = data?.imageurl ?? ''; 
    this.customer = data?.customer ? new Customer(data.customer) : new Customer();
  }
}
