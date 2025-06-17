import { Component } from '@angular/core';
import { UserService } from '../service/userloginservice';

@Component({
  selector: 'app-listenwelocome',
  imports: [],
  templateUrl: './listenwelocome.html',
  styleUrl: './listenwelocome.css'
})
export class Listenwelocome {
usrname:string|null = "";

  constructor(private userService:UserService)
  {
    this.userService.username$.subscribe(
      {
       next:(value) =>{
          this.usrname = value ;
        },
        error:(err)=>{
          // alert(err);
        }
      }
    )
  }
}
