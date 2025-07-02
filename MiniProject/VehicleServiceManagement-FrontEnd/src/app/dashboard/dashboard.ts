import { Component } from '@angular/core';
import { DashBoardService } from '../Services/DashBoardService';
import { catchError, debounceTime, of, Subject, switchMap, tap } from 'rxjs';
import { CommonModule } from '@angular/common';
import { CanvasJSAngularChartsModule } from '@canvasjs/angular-charts';
import { FormsModule } from '@angular/forms';
import { UserService } from '../Services/UserServices';
import { billService } from '../Services/BillService';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, CanvasJSAngularChartsModule],
  templateUrl: './dashboard.html',
  styleUrls: ['./dashboard.css']
})
export class Dashboard {
  dashboardData: any;
  dashboardKeys: string[] = [];

  phoneNo = '';
  from = '';
  to = '';
  loading = true;
  showMessage = false;
  message = '';
 email =localStorage.getItem("email")||''
  searchSubject = new Subject<{ phoneNo: string; from?: string; to?: string }>();
  bills:any[]=[]
  bookingOptions: any;
  serviceOptions: any;
  mechanicOptions: any;
  billOptions: any;
  role=""
  
  constructor(private service: DashBoardService,private userService:UserService,private bService:billService) {
     this.role=userService.getRole();
     this.userService.getProfile(this.email).subscribe(
      {
        next:(data:any)=>
        {
          if(this.role=="Customer" || this.role=="Mechanic")
          {
          this.phoneNo=data.phone
          console.log(this.phoneNo)
           this.applyFilters();
          }
        }
      }
     )
    this.searchSubject.pipe(
      debounceTime(300),
      tap(() => this.loading = true),
      switchMap(({ phoneNo, from, to }) =>
        this.service.getAllAnalytics(phoneNo, from, to).pipe(
          catchError(err => {
            this.showMessage = true;
            console.log(err)
            this.message = 'No Data Found';
            return of(null);
          })
        )
      ),
      tap(() => this.loading = false)
    ).subscribe({
      next: (data) => {
        if (data) {
          this.dashboardData = data;
          this.updateDashboardKeys();
          this.prepareCharts(data);
        }
      }
    });

    this.applyFilters();

  
  }
  

 
  applyFilters() {
    this.showMessage = false;
    this.message = '';
    this.searchSubject.next({ phoneNo: this.phoneNo, from: this.from, to: this.to });
      this.bService.showAllBills(this.phoneNo,1,10).subscribe(
    {
   next:(data:any)=>
   {
      this.bills=data
      this.bills=this.bills.filter(b=>b.status=="Approved")
   }
    }
    )
  }

  private updateDashboardKeys() {
    if (!this.dashboardData) return;


    this.dashboardKeys = Object.keys(this.dashboardData)
      .filter(key => typeof this.dashboardData[key] === 'number');
  }

  formatKey(key: string): string {
    return key
      .replace(/([A-Z])/g, ' $1')
      .replace(/^./, str => str.toUpperCase());
  }

  private prepareCharts(data: any) {
    const bookingData = Object.entries(data.bookingCountsByDate || {})
      .map(([date, count]: any) => ({ type: '',x: new Date(date), y: count })).sort((a, b) => a.x.getTime() - b.x.getTime());;
    this.bookingOptions = this.generateChartOption('', 'Bookings', 'blue', bookingData);

    const serviceData = Object.entries(data.serviceCountsByDate || {})
      .map(([date, count]: any) => ({ x: new Date(date), y: count })).sort((a, b) => a.x.getTime() - b.x.getTime());;
    this.serviceOptions = this.generateChartOption('', 'Services', 'green', serviceData);

   const blueShades = ['blue', 'green', 'red'];
const mechSeries = ['Active', 'Completed', 'Aborted'];

const mechanicData = mechSeries.map((series, i) => ({
  type: 'column',
  name: series,
  showInLegend: true,
  color: blueShades[i % blueShades.length],  
  dataPoints: data.mechanicServiceStats.map((m: any) => ({
    label: m.mechanicName,
    y: Number(m[series.toLowerCase()])
  }))
}));

this.mechanicOptions = {
  animationEnabled: true,
  exportEnabled: true,
  
  axisY: { title: 'Service Count', gridThickness: 0 },
  data: mechanicData
};


    const billData = Object.entries(data.customerBillSummary || {})
      .map(([phone, total]: any) => ({ label: phone, y: Number(total) }));
    this.billOptions = {
      animationEnabled: true,
      exportEnabled: true,
      
      axisY: { title: 'Amount',gridThickness: 0 },
     
    
      data: [{
        type: 'column',
        name: 'Total Bill',
        showInLegend: false,
        color:"green",
        dataPoints: billData
      }]
    };
  }

  private generateChartOption(title: string, legend: string, color: string, dataPoints: { x: Date, y: number }[]) {
    return {
     
      animationEnabled: true,
      exportEnabled: true,
      theme: 'dark',
      title: { text: title },
      axisX: { title: 'Date', valueFormatString: 'DD MMM YYYY' },
      axisY: { title: 'Count' ,gridThickness: 0},
      data: [{ type: 'spline', name: legend, showInLegend: true, color, dataPoints }]
    };
  }
}
