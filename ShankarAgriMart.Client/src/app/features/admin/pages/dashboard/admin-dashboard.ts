import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { CommonModule } from '@angular/common';

import { AdminDashboardService } from '../../services/admin-dashboard.service';
import { AdminDashboard } from '../../models/admin-dashboard.model';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.css'
})
export class AdminDashboardComponent implements OnInit {

  private readonly dashboardService = inject(
    AdminDashboardService
  );

  private readonly changeDetectorRef = inject(
    ChangeDetectorRef
  );

  dashboard: AdminDashboard | null = null;

  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.loadDashboard();
  }

  private loadDashboard(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.dashboardService.getDashboard().subscribe({

      next: (response) => {

        if (response.success) {
          this.dashboard = response.data;
        } else {
          this.errorMessage =
            response.message || 'Unable to load dashboard.';
        }

        this.isLoading = false;

        // Explicitly notify Angular that component state changed.
        this.changeDetectorRef.detectChanges();
      },

      error: (error) => {

        console.error(
          'Admin dashboard error:',
          error
        );

        this.errorMessage =
          'Unable to load dashboard data. Please try again.';

        this.isLoading = false;

        // Explicitly notify Angular.
        this.changeDetectorRef.detectChanges();
      }
    });
  }
}