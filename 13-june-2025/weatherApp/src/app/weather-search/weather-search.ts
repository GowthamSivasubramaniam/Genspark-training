import { Component } from '@angular/core';
import { WeatherService } from '../services/WeatherApiService';

@Component({
  selector: 'app-weather-search',
  imports: [],
  templateUrl: './weather-search.html',
  styleUrl: './weather-search.css'
})
export class WeatherSearch {

 constructor(private ser:WeatherService){}
  handleSearch(value:string){
  this.ser.getWeather(value);
}
 
}
