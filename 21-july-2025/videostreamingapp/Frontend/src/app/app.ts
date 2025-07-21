import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Videostream } from './videostream/videostream';

@Component({
  selector: 'app-root',
  imports: [Videostream],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected title = 'Frontend';
}
