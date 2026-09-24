import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';

import {
  AdminDashboardApiResponse
} from '../models/admin-dashboard.model';

@Injectable({
  providedIn: 'root'
})
export class AdminDashboardService {

  private readonly api = inject(ApiService);

  getDashboard(): Observable<AdminDashboardApiResponse> {
    return this.api.get<AdminDashboardApiResponse>(
      'admin/dashboard'
    );
  }
}