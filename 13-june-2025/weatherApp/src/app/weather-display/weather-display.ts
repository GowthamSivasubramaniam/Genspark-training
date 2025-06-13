import { Component } from '@angular/core';
import { WeatherService } from '../services/WeatherApiService';

@Component({
  selector: 'app-weather-display',
  standalone: true,
  imports: [],
  templateUrl: './weather-display.html',
  styleUrls: ['./weather-display.css']
})
export class WeatherDisplay {
  condition: any = null;
  history: string[] = [];

  constructor(private ser: WeatherService) {
    
    this.ser.weather$.subscribe({
      next: (value) => {
        if(value .success != null && value.success == false)
         alert("Invalid city");
        this.condition = value;
      },
      error: (err) => {
       alert(err);
      }
    });

    
    this.ser.history$.subscribe({
      next: (data) => {
        this.history = data;
      }
    });
  }

  onSelectCity(city: string) {
    if (city) {
      this.ser.getWeather(city);
    }
  }
}
