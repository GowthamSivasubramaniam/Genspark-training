import { Component } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { userService } from '../Services/UserService';
import { last } from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class Register {
  registerForm:FormGroup;
  userData:any;
  constructor(private service: userService,private route:Router) {
  this.registerForm = new FormGroup({
  fname:new FormControl(null,[Validators.required]),
  lname:new FormControl(null,[Validators.required]),
  age:new FormControl(null,[Validators.required]),
  role:new FormControl(null,[Validators.required]),
  state:new FormControl(null,[Validators.required]),
  gender:new FormControl(null,[Validators.required])
  })
  }
public get fname() : any {
  return this.registerForm.get("fname")
}

public get lname() : any {
  return this.registerForm.get("lname")
}
public get age() : any {
  return this.registerForm.get("age")
}

public get role() : any {
  return this.registerForm.get("role")
}
public get state() : any {
  return this.registerForm.get("state")
}

public get gender() : any {
  return this.registerForm.get("gender")
}


Adduser() {
  // console.log(this.pass.errors);
    this.userData = {
    firstname: this.fname.value,
    lastname: this.lname.value,
    age: this.age.value,
    gender: this.gender.value,
    state: this.state.value,
    role: this.role.value
  };
    if(this.registerForm.invalid)
    return;
    this.service.callUserAddAPI(this.userData).subscribe({
      next:(data:any) =>
      {console.log(data)}
  })
  }
}

