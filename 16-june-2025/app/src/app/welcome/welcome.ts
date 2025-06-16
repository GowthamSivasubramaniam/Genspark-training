import { Component } from '@angular/core';

@Component({
  selector: 'app-welcome',
  imports: [],
  templateUrl: './welcome.html',
  styleUrl: './welcome.css'
})
export class Welcome {
   uname:string =" ";
   constructor()
   {
    const user = localStorage.getItem('user');
    
    if(user)
    {
      this.uname = JSON.parse(user).name;
      sessionStorage.setItem("name",this.uname);
    }
   }

}
