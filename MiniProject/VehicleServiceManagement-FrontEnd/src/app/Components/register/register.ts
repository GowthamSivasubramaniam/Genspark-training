import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserService } from '../../Services/UserServices';
import { emailValidator, nameValidator, passwordValidator, phoneValidator } from '../../Misc/Validations';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  
@Input() isChild: boolean = false; 
  @Output() formSubmitted = new EventEmitter<any>(); 
  data:any

  registerForm:FormGroup
  constructor(private service:UserService)
  {
    this.registerForm = new FormGroup(
      {
        Name: new FormControl(null,[Validators.required,nameValidator()]),
        Email: new FormControl(null,[Validators.required,emailValidator()]),
        PhoneNumber: new FormControl(null,[Validators.required,phoneValidator()]),
        Password: new FormControl(null,[Validators.required,passwordValidator()]),
        ConfirmPassword : new FormControl(null,[Validators.required]),
       
      }
    )
  }
  
  public get Name() : any {
    return this.registerForm.get("Name");
  }
  public get Email() : any {
    return this.registerForm.get("Email");
  }
  public get PhoneNumber() : any {
    return this.registerForm.get("PhoneNumber");
  }
  public get Password() : any {
    return this.registerForm.get("Password");
  }
  public get ConfirmPassword() : any {
    return this.registerForm.get("ConfirmPassword");
  }

  showMessage:boolean=false
  message:string=""

  registerUser()
  {
this.data =  
{
 name:this.Name.value,
 email:this.Email.value,
 phone:this.PhoneNumber.value,
 password:this.Password.value
}
if (this.isChild) {
     
      this.formSubmitted.emit(this.data);
      return;
    }
  this.service.registerUser(this.data).subscribe({
  next: (res) => {
    this.showMessage = true;
    this.message = "Registration Successful";
  },
  error: (err) => {
    this.showMessage = true;
    this.message = "Registration Failed, Try again.";
    console.error(err);
  }
});

  
}


}
  



