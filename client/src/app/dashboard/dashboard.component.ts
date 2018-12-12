import { Component, OnInit } from '@angular/core';
import { DashboardService } from './dashboard-service';
import { CareHome } from './careHome';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements OnInit {

  careHomes: CareHome[];

  constructor(private dashboardService: DashboardService) { }

  ngOnInit() {
    this.getCareHomes();
  }

  private getCareHomes(): void {
    this.dashboardService.getCategories()
      .subscribe(careHomes => this.careHomes = careHomes);
  }
}
