import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class WeatherService {
  private weatherSubject = new BehaviorSubject<any>(null);
  weather$ = this.weatherSubject.asObservable();

  private historySubject = new BehaviorSubject<string[]>(this.getHistoryFromStorage());
  history$ = this.historySubject.asObservable();

  constructor(private http: HttpClient) {}

  getWeather(city: string) {
    const url = `http://api.weatherstack.com/current?access_key=582baf3918252cb5fa6221ba14fe9103&query=${city}`;

    this.http.get(url).subscribe({
      next: (data: any) => {
        this.weatherSubject.next(data);
        console.log(data);
        this.updateSearchHistory(city);
      },
      error: (err) => {
        console.error('API error', err);
        this.weatherSubject.next(null);
      }
    });
  }

  private updateSearchHistory(city: string) {
    let history = this.historySubject.value || [];
    history = [city, ...history.filter(c => c !== city)].slice(0, 5);
    localStorage.setItem('weather_history', JSON.stringify(history));
    this.historySubject.next(history);
  }

  private getHistoryFromStorage(): string[] {
    const history = localStorage.getItem('weather_history');
    return history ? JSON.parse(history) : [];
  }
}
