import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AdminOrderService } from '../../../services/admin-order.service';
import { AdminOrder } from '../../../models/admin-order.model';


@Component({
  selector: 'app-admin-orders',
  standalone: true,
  imports: [
    CommonModule,
    DatePipe,
    RouterLink
  ],
  templateUrl: './admin-orders.html',
  styleUrl: './admin-orders.css'
})
export class AdminOrdersComponent implements OnInit {

  private readonly orderService = inject(AdminOrderService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  orders: AdminOrder[] = [];

  isLoading = true;
  errorMessage = '';

  selectedStatus = 0;

  ngOnInit(): void {
    this.loadOrders();
  }

  /**
   * Load all orders from backend.
   */
  loadOrders(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.orderService.getAllOrders().subscribe({
      next: (response) => {
        console.log('Admin orders loaded:', response);

        this.orders = response.data ?? [];
        this.isLoading = false;

        this.changeDetector.detectChanges();
      },

      error: (error) => {
        console.error('Admin orders API error:', error);

        this.errorMessage =
          error?.error?.message ||
          'Unable to load orders. Please try again later.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }
    });
  }

  /**
   * Filter orders by status.
   *
   * 0 = All
   * 1 = Pending
   * 2 = Confirmed
   * 3 = Shipped
   * 4 = Delivered
   * 5 = Cancelled
   */
  get filteredOrders(): AdminOrder[] {
    if (this.selectedStatus === 0) {
      return this.orders;
    }

    return this.orders.filter(
      order => order.orderStatus === this.selectedStatus
    );
  }

  /**
   * Change selected status filter.
   */
  setStatusFilter(status: number): void {
    this.selectedStatus = status;
  }

  /**
   * Convert order status enum to readable text.
   */
  getOrderStatus(status: number): string {
    switch (status) {
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

  /**
   * Return Bootstrap/custom CSS class
   * based on order status.
   */
  getOrderStatusClass(status: number): string {
    switch (status) {
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

  /**
   * Convert payment method to readable text.
   */
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

  /**
   * Convert payment status to readable text.
   */
  getPaymentStatus(status: number): string {
    return status === 1 ? 'Pending' : 'Paid';
  }

  /**
   * Payment status CSS class.
   */
  getPaymentStatusClass(status: number): string {
    return status === 1
      ? 'payment-pending'
      : 'payment-paid';
  }
}