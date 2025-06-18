import { Component } from '@angular/core';
import { NgChartsModule } from 'ng2-charts';
import { userService } from '../Services/UserService';
import { Router } from '@angular/router';
import { MapComponent } from "../map/map";
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [NgChartsModule, MapComponent, FormsModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard {
  userdata: any[] = [];
  roleChartData: any;
  genderChartData: any;
  stateChartData: any;

  selectedRole: string | null = null;
  selectedGender: string | null = null;

  stateCounts: { [state: string]: number } = {};

  constructor(private service: userService, private route: Router) {}

  ngOnInit(): void {
    this.service.getusers().subscribe({
      next: (res: any) => {
        this.userdata = res.users;
        this.createChart();
      },
      error: (err) => {
        console.error('Failed to load user data', err);
      }
    });
  }

  applyFilters(): void {
    const filteredData = this.getFilteredUsers(this.selectedRole, this.selectedGender);
    this.createChart(filteredData);
  }

  getFilteredUsers(role?: string | null, gender?: string | null): any[] {
    return this.userdata.filter(user =>
      (role == null || user.role === role) &&
      (gender == null || user.gender === gender)
    );
  }

  createChart(data: any[] = this.userdata) {
    this.loadData(data, 'role', 'roleChartData');
    this.loadData(data, 'gender', 'genderChartData');

    const statesArray = data
      .map(u => u.address?.state)
      .filter((s: string | undefined) => s && s.trim() !== '') as string[];

    this.stateCounts = this.countBy(statesArray);
    this.loadData(statesArray, null, 'stateChartData');
  }

  private countBy(array: string[]): { [key: string]: number } {
    const countMap: { [key: string]: number } = {};
    for (const value of array) {
      countMap[value] = (countMap[value] || 0) + 1;
    }
    return countMap;
  }

  private loadData(
    source: any[],
    key: string | null,
    chartDataProp: 'roleChartData' | 'genderChartData' | 'stateChartData'
  ) {
    const countMap: { [key: string]: number } = {};

    if (key) {
      for (const item of source) {
        const value = item[key];
        if (value) countMap[value] = (countMap[value] || 0) + 1;
      }
    } else {
      for (const value of source) {
        if (value) countMap[value] = (countMap[value] || 0) + 1;
      }
    }

    const labels = Object.keys(countMap);
    const data = Object.values(countMap);
    const backgroundColor = labels.map(() => this.getRandomColor());

    const chartLabel =
      chartDataProp === 'roleChartData'
        ? 'Roles'
        : chartDataProp === 'genderChartData'
        ? 'Genders'
        : 'States';

    this[chartDataProp] = {
      labels,
      datasets: [
        {
          label: chartLabel,
          data,
          backgroundColor
        }
      ],
      options: {
        scales: {
          x: {
            grid: { display: false }
          },
          y: {
            grid: { display: false }
          }
        }
      }
    };
  }

  private getRandomColor(): string {
    const letters = '0123456789ABDCE';
    let color = '#';
    for (let i = 0; i < 6; i++) {
      color += letters[Math.floor(Math.random() * 16)];
    }
    return color;
  }
}
