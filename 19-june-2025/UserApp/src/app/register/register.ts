import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { addUser } from '../ngrx/users.actions';
import { User } from '../models/User';
import { Observable } from 'rxjs';
import { Store } from '@ngrx/store';
import { UserNameValidator } from '../Misc/textValidator';
import { emailValidator } from '../Misc/emailValidator';
import { ConfirmPasswordvalidator } from '../Misc/ConfirmPasswordValidator';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  registerForm:FormGroup;
  userData:any;
  isSuccess:boolean =false;
   
  constructor(private route:Router,private store:Store) {
  this.registerForm = new FormGroup({
  uname:new FormControl(null,[Validators.required,UserNameValidator()]),
  email:new FormControl(null,[Validators.required,emailValidator()]),
  role:new FormControl("User",[Validators.required]),
  password:new FormControl(null,[Validators.required]),
  confirmPassword:new FormControl(null,[Validators.required])
 
  },{ validators: ConfirmPasswordvalidator() })



  }
public get uname() : any {
  return this.registerForm.get("uname")
}

public get email() : any {
  return this.registerForm.get("email")
}

public get role() : any {
  return this.registerForm.get("role")
}

public get password() : any {
  return this.registerForm.get("password")
}
public get confirmPassword() : any {
  return this.registerForm.get("confirmPassword")
}


Adduser() {

    this.userData = {
    username: this.uname.value,
    email:this.email.value,
    password:this.password.value,
    Role: this.role.value
  };
    if(this.registerForm.invalid)
    {
    return;
    }
   this.store.dispatch(addUser({ user: this.userData }));
   this.isSuccess=true;
  }
}