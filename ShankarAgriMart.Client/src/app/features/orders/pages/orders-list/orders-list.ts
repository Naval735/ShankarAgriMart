import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { DatePipe } from '@angular/common';

import {
  RouterLink
} from '@angular/router';

import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order.model';

@Component({
  selector: 'app-orders-list',
  imports: [
    RouterLink,
    DatePipe
  ],
  templateUrl: './orders-list.html',
  styleUrl: './orders-list.css'
})
export class OrdersListComponent implements OnInit {

  private readonly orderService = inject(OrderService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  orders: Order[] = [];

  isLoading = true;
  errorMessage = '';

  ngOnInit(): void {
    this.loadOrders();
  }

  private loadOrders(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.orderService
      .getOrders()
      .subscribe({

        next: (orders: Order[]) => {

          console.log(
            'Orders loaded:',
            orders
          );

          this.orders = orders;
          this.isLoading = false;

          this.changeDetector.detectChanges();
        },

        error: (error) => {

          console.error(
            'Orders API error:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Unable to load your orders. Please try again later.';

          this.isLoading = false;

          this.changeDetector.detectChanges();
        }

      });
  }

  getPaymentMethod(
    paymentMethod: number | string | null
  ): string {

    if (
      paymentMethod === 2 ||
      paymentMethod === 'CashOnDelivery' ||
      paymentMethod === 'COD'
    ) {
      return 'Cash on Delivery';
    }

    return 'Online Payment';
  }

  getPaymentStatus(
    paymentStatus: number
  ): string {

    return paymentStatus === 1
      ? 'Pending'
      : 'Paid';
  }

  getOrderStatus(
    orderStatus: number
  ): string {

    switch (orderStatus) {

      case 1:
        return 'Pending';

      case 2:
        return 'Confirmed';

      case 3:
        return 'Shipped';

      case 4:
        return 'Delivered';

      case 5:
        return 'Cancelled';

      default:
        return 'Unknown';
    }
  }

  getOrderStatusClass(
    orderStatus: number
  ): string {

    switch (orderStatus) {

      case 1:
        return 'status-pending';

      case 2:
        return 'status-confirmed';

      case 3:
        return 'status-shipped';

      case 4:
        return 'status-delivered';

      case 5:
        return 'status-cancelled';

      default:
        return 'status-pending';
    }
  }

  getPaymentStatusClass(
    paymentStatus: number
  ): string {

    return paymentStatus === 1
      ? 'payment-pending'
      : 'payment-paid';
  }
}