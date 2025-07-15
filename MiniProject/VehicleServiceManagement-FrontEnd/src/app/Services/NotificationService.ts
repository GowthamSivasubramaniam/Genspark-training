import { Injectable } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { BehaviorSubject } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private hubConnection!: signalR.HubConnection;
  private _notifications = new BehaviorSubject<string[]>([]);
  public notifications$ = this._notifications.asObservable();

  constructor() {
    this.loadFromStorage();
    this.startConnection();
  }

  private startConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('https://localhost:7176/notificationHub')
      .withAutomaticReconnect()
      .build();

    this.hubConnection.start()
      .then(() => console.log('SignalR connected'))
      .catch(err => console.error('SignalR error', err));

    this.hubConnection.on('ReceiveNotification', (message: string) => {
        message="Slot Booked  "+message
      const updated = [message, ...this._notifications.value];
      this._notifications.next(updated);
      localStorage.setItem('notifications', JSON.stringify(updated));
    });
  }

  private loadFromStorage() {
    const saved = localStorage.getItem('notifications');
    const list = saved ? JSON.parse(saved) : [];
    this._notifications.next(list);
  }

  clearAll() {
    this._notifications.next([]);
    localStorage.removeItem('notifications');
  }
}
