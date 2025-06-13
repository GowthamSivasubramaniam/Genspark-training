import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { WeatherSearch } from "./weather-search/weather-search";
import { WeatherDisplay } from "./weather-display/weather-display";

@Component({
  selector: 'app-root',
  imports: [WeatherSearch, WeatherDisplay],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'weatherApp';
}
