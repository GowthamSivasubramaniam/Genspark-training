import { Component, inject } from '@angular/core';
import { UserService } from '../../Services/UserServices';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-profile',
  imports: [FormsModule,CommonModule],
  templateUrl: './profile.html',
  styleUrl: './profile.css'
})
export class Profile {
   data:any
   email:any
   img:string ='assets/user.jpg'
   service = inject(UserService)
   showMessage = false;
  message = '';
  toast: string = ""; // Add this variable at the top with other properties
   ngOnInit() : void
   {
    this.email = localStorage.getItem('email')
       this.service.getProfile(this.email).subscribe
       (
        {
          next:(data:any)=>
          {
            this.toast = "success"; // <-- set toast to success
            this.data = data;
            
          },
          error: (err) => {
            this.toast = "error"; // <-- set toast to error
          console.error('Error fetching profile:', err);
        }
        }
       )
   }
   isEdited: boolean = false;

onEdit() {
  this.isEdited = true;
}

updateProfile() {
  this.service.updateProfile({"name":this.data.name ,"phone":this.data.phone },this.email).subscribe({
    next: () => {
      this.toast = "success";
      this.isEdited = false;
      this.showMessage = false;
      setTimeout(() => {
        this.showMessage = true;
        this.message = "Updated successfully";
      }, 1000);
    },
    error: (err:any) => {
      this.toast = "error";
      console.error("Update failed", err);
      this.isEdited = false;
      this.showMessage = false; 
      setTimeout(() => {
        this.showMessage = true; 
        this.message = err.error?.message || "Something went wrong";
      }, 1000)
    }
  });
}
}
