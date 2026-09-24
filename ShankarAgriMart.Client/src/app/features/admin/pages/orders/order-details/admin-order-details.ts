import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';
import { CommonModule, DatePipe, DecimalPipe } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AdminOrderService } from '../../../services/admin-order.service';
import { AdminOrder } from '../../../models/admin-order.model';

@Component({
  selector: 'app-admin-order-details',
  standalone: true,
 imports: [
  CommonModule,
  DatePipe,
  DecimalPipe,
  RouterLink,
  FormsModule
],
  templateUrl: './admin-order-details.html',
  styleUrl: './admin-order-details.css'
})
export class AdminOrderDetailsComponent implements OnInit {

  private readonly route = inject(ActivatedRoute);
  private readonly orderService = inject(AdminOrderService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  order: AdminOrder | null = null;

  isLoading = true;
  isUpdatingStatus = false;

  errorMessage = '';
  statusMessage = '';

  selectedStatus = 0;

  ngOnInit(): void {
    const orderId = Number(
      this.route.snapshot.paramMap.get('id')
    );

    if (!orderId) {
      this.errorMessage = 'Invalid order ID.';
      this.isLoading = false;
      return;
    }

    this.loadOrder(orderId);
  }

  private loadOrder(orderId: number): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.orderService.getOrderById(orderId).subscribe({
      next: (response) => {
        console.log('Admin order details:', response);

        this.order = response.data;

        // Keep the dropdown synchronized
        // with the current backend status.
        this.selectedStatus = this.order.orderStatus;

        this.isLoading = false;

        this.changeDetector.detectChanges();
      },

      error: (error) => {
        console.error('Admin order details error:', error);

        this.errorMessage =
          error?.error?.message ||
          'Unable to load order details.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }
    });
  }

  updateStatus(): void {

    if (!this.order) {
      return;
    }

    const newStatus = Number(this.selectedStatus);

    if (!newStatus) {
      return;
    }

    // No API call if nothing changed.
    if (newStatus === this.order.orderStatus) {
      return;
    }

    this.isUpdatingStatus = true;
    this.statusMessage = '';
    this.errorMessage = '';

    this.orderService
      .updateOrderStatus(this.order.id, newStatus)
      .subscribe({

        next: (response) => {

          console.log(
            'Order status updated:',
            response
          );

          this.order = response.data;

          this.selectedStatus =
            this.order.orderStatus;

          this.statusMessage =
            'Order status updated successfully.';

          this.isUpdatingStatus = false;

          this.changeDetector.detectChanges();
        },

        error: (error) => {

          console.error(
            'Order status update error:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Unable to update order status.';

          // Restore original status in dropdown.
          this.selectedStatus =
            this.order?.orderStatus ?? 0;

          this.isUpdatingStatus = false;

          this.changeDetector.detectChanges();
        }
      });
  }

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

  getPaymentStatus(status: number): string {
    return status === 1 ? 'Pending' : 'Paid';
  }

  getPaymentStatusClass(status: number): string {
    return status === 1
      ? 'payment-pending'
      : 'payment-paid';
  }
}