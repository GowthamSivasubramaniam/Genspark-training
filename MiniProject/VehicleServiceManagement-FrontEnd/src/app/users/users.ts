import { Component } from '@angular/core';
import { UserService } from '../Services/UserServices';
import { Register } from "../Components/register/register";
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-users',
  imports: [Register,FormsModule],
  templateUrl: './users.html',
  styleUrl: './users.css'
})
export class Users {
   showForm:boolean =false
   selectedRole: string = 'Mechanic';
   showMech:boolean =true
   CustomerData:any[]=[]
   MechanicData:any[]=[]
    showMessage = false;
  message = '';
  constructor(private service: UserService) {

    this.service.getAllCustomers().subscribe(
      {
        next:(data:any)=>
        {
          console.log(data)
            this.CustomerData=data
        }
      }
    )
    this.service.getAllMechanics().subscribe(
      {
        next:(data:any)=>
        {
          console.log(data)
            this.MechanicData=data
        }
      }
    )

  }
  clickshowform()
  {
    this.showForm=true
  }
  hideform()
  {
    this.showForm=false
  }
  registerMechanic(data: any) {
    console.log("Mechanic Data Received:", data);
    this.service.registerMechanic(data).subscribe({
      next: () => {
       this.showMessage=true
       this.message="Registration successfull"
      },
      error: () => {
        this.showMessage=true
       this.message="Registration Failed"
      }
    });
  }
  deactivateMechanic(data:any)
  {
   
    this.service.deactivateProfile(data,data.email).subscribe(
    {
       next: (val:any) => {
        data.status=val.status
        
         this.showMessage=true
     this.message="Updation successfull"
       
      },
      error: () => {
        this.showMessage=true
       this.message="Updation failed"
      }
    })
  }
 changeView()
 {
  if(this.selectedRole ==="Mechanic")
  {
    this.showMech=true;
  }
  else
  {
    this.showMech=false;
  }
 }
filter = {
  name: '',
  email: '',
  phone: '',
  status: ''
};

get filteredMechanics() {
  return this.MechanicData.filter(item =>
    (!this.filter.name || item.name.toLowerCase().includes(this.filter.name.toLowerCase())) &&
    (!this.filter.email || item.email.toLowerCase().includes(this.filter.email.toLowerCase())) &&
    (!this.filter.phone || item.phone.includes(this.filter.phone)) &&
    (!this.filter.status || item.status === this.filter.status)
  );
}

get filteredCustomers() {
  return this.CustomerData.filter(item =>
    (!this.filter.name || item.name.toLowerCase().includes(this.filter.name.toLowerCase())) &&
    (!this.filter.email || item.email.toLowerCase().includes(this.filter.email.toLowerCase())) &&
    (!this.filter.phone || item.phone.includes(this.filter.phone)) &&
    (!this.filter.status || item.status === this.filter.status)
  );
}

}
