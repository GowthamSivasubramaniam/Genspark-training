import { Component, Query } from '@angular/core';
import { debounceTime, distinctUntilChanged, filter, Observable, Subject, switchMap, tap } from 'rxjs';
import { User } from '../models/User';
import { Store } from '@ngrx/store';
import { selectAllUsers, selectUserError, selectUserLoading } from '../ngrx/user.selector';
import { AsyncPipe } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-list',
  imports: [FormsModule],
  templateUrl: './user-list.html',
  styleUrl: './user-list.css'
})
export class UserList {

  searchQuery:string =""
  loading:boolean = false;
  userdata:User[] = [];
  private searchSubject = new Subject<string>();
  constructor(private store:Store)
  {
   
  }
  handleSearch()
   {
     this.searchSubject.next(this.searchQuery)
   }
   ngOnInit(): void {
    this.searchSubject.pipe(
      debounceTime(500),
      distinctUntilChanged(),
      tap(() => this.loading = true),
      switchMap(query =>
        this.store.select(selectAllUsers).pipe(
          tap(() => this.loading = false),
          tap(users => {
            console.log(users)
            this.userdata = users.filter(user =>
              user.username.toLowerCase().includes(query.toLowerCase())
            );
          })
        )
      )
    ).subscribe();

      this.handleSearch()
   }

  
}

