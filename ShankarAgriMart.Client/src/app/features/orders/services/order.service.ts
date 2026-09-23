import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import {
  CreateOrderRequest,
  Order
} from '../models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private readonly api = inject(ApiService);

  createOrder(request: CreateOrderRequest): Observable<Order> {
    return this.api.post<Order>('Order', request);
  }

  getOrders(): Observable<Order[]> {
    return this.api.get<Order[]>('Order');
  }

  getOrderById(id: number): Observable<Order> {
    return this.api.get<Order>(`Order/${id}`);
  }
}