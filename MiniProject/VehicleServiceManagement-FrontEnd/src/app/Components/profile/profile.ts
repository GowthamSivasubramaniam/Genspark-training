import { Component, inject } from '@angular/core';
import { UserService } from '../../Services/UserServices';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-profile',
  imports: [FormsModule],
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
   ngOnInit() : void
   {
    this.email =localStorage.getItem('email')
       this.service.getProfile(this.email).subscribe
       (
        {
          next:(data:any)=>
          {
            this.data = data;
            
          },
          error: (err) => {
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
      this.isEdited = false;
      this.showMessage=true
     this.message = "Updated successfully"
    },
    error: (err:any) => {
      console.error("Update failed", err);
      this.isEdited = false;
      this.showMessage=true
     this.message = err.error.message || "Something went wrong"
    }
  });
}
}
