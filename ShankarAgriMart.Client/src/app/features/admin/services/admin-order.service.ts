import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import {
  AdminOrder,
  AdminOrderApiResponse
} from '../models/admin-order.model';

@Injectable({
  providedIn: 'root'
})
export class AdminOrderService {

  private readonly api = inject(ApiService);

  /**
   * Get all customer orders for admin.
   */
  getAllOrders(): Observable<AdminOrderApiResponse<AdminOrder[]>> {
    return this.api.get<AdminOrderApiResponse<AdminOrder[]>>(
      'admin/orders'
    );
  }

  /**
   * Get a specific order by ID.
   */
  getOrderById(
    orderId: number
  ): Observable<AdminOrderApiResponse<AdminOrder>> {
    return this.api.get<AdminOrderApiResponse<AdminOrder>>(
      `admin/orders/${orderId}`
    );
  }

  /**
   * Update order status.
   *
   * Backend expects OrderStatus enum value:
   * 1 = Pending
   * 2 = Confirmed
   * 3 = Shipped
   * 4 = Delivered
   * 5 = Cancelled
   */
  updateOrderStatus(
    orderId: number,
    status: number
  ): Observable<AdminOrderApiResponse<AdminOrder>> {
    return this.api.put<AdminOrderApiResponse<AdminOrder>>(
      `admin/orders/${orderId}/status`,
      status
    );
  }
}