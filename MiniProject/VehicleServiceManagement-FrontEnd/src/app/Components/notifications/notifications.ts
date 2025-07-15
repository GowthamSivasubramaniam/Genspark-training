import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { NotificationService } from '../../Services/NotificationService';
import {  Subscription } from 'rxjs';
@Component({
  selector: 'app-notifications',
  imports: [],
  templateUrl: './notifications.html',
  styleUrl: './notifications.css'
})
export class Notifications implements OnInit,OnDestroy {
dropdownVisible = false;
  notifications: string[] = [];
  private sub!: Subscription;

  constructor(public notificationService: NotificationService) {}

  ngOnInit() {
    this.sub = this.notificationService.notifications$.subscribe((list) => {

      this.notifications=list
    });
  }

  ngOnDestroy() {
    this.sub.unsubscribe();
  }

  toggleDropdown() {
    this.dropdownVisible = !this.dropdownVisible;
  }

  clearAll() {
    this.notificationService.clearAll();
    this.dropdownVisible = false;
  }
}
