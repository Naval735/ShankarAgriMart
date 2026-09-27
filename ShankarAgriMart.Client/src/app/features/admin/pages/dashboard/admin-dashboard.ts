import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  RouterLink
} from '@angular/router';

import {
  AdminDashboardService
} from '../../services/admin-dashboard.service';

import {
  AdminDashboard
} from '../../models/admin-dashboard.model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,

  imports: [
    CommonModule,
    RouterLink
  ],

  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboardComponent
  implements OnInit {

  private readonly dashboardService =
    inject(AdminDashboardService);

  private readonly changeDetectorRef =
    inject(ChangeDetectorRef);

  dashboard: AdminDashboard | null = null;

  isLoading = true;

  errorMessage = '';

  ngOnInit(): void {
    this.loadDashboard();
  }

  loadDashboard(): void {

    this.isLoading = true;

    this.errorMessage = '';

    this.changeDetectorRef.detectChanges();

    this.dashboardService
      .getDashboard()
      .subscribe({

        next: (response) => {

          if (response.success) {

            this.dashboard =
              response.data;

          } else {

            this.dashboard = null;

            this.errorMessage =
              response.message ||
              'Unable to load dashboard.';
          }

          this.isLoading = false;

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {

          console.error(
            'Admin dashboard error:',
            error
          );

          this.dashboard = null;

          this.errorMessage =
            'Unable to load dashboard data. Please try again.';

          this.isLoading = false;

          this.changeDetectorRef.detectChanges();
        }

      });
  }
}