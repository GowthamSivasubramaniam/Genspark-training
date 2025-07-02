import { Routes } from '@angular/router';
import { Register } from './Components/register/register';
import { Login } from './Components/login/login';
import { Main } from './main/main';
import { Protect } from './authGaurd';
import { Profile } from './Components/profile/profile';

import { Bookings } from './Components/bookings/bookings';
import { Users } from './users/users';
import { Categories } from './Components/categories/categories';
import { Registrations } from './registrations/registrations';
import { Bills } from './bills/bills';
import { Dashboard } from './dashboard/dashboard';
import { App } from './app';
// import { Dashboard } from './dashboard/dashboard';

export const routes: Routes = [
  {path: 'home',component:App},

  {
    path: 'main',
    component: Main,
    canActivate: [Protect],
    children: [
   
      { 
        path: 'profile', component: Profile 
      },
      {
        path:'booking',component:Bookings
      },
      {
        path:'users',component:Users
      },
      {
        path:'categories',component:Categories
      },
      {
        path:'registrations',component:Registrations
      },
      {
        path:'bills',component:Bills
      },
      {
        path:'',component:Dashboard
      }
     
    ]
  },

  { path: '', redirectTo: 'main', pathMatch: 'full' },  
  { path: '**', redirectTo: 'main' } 
];
