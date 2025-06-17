import { Component, inject } from '@angular/core';
import { ActivatedRoute, RouterLink, RouterModule } from '@angular/router';

@Component({
  selector: 'app-about',
  imports: [RouterLink,RouterModule],
  templateUrl: './about.html',
  styleUrl: './about.css'
})
export class About {
  route = inject(ActivatedRoute);
  user:string = this.route.snapshot.params['un'] as string;
}
