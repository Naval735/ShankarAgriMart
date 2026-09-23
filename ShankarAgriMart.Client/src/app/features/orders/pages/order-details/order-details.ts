import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { DatePipe } from '@angular/common';

import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';

import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order.model';

import { PaymentService } from '../../../payment/services/payment.service';

import { AddressService } from '../../../checkout/services/address.service';
import { Address } from '../../../checkout/models/address.model';

import {
  RazorpayOptions,
  RazorpayResponse
} from '../../../../core/models/razorpay.model';

import { environment } from '../../../../../environments/environment.development';

@Component({
  selector: 'app-order-details',
  imports: [RouterLink, DatePipe],
  templateUrl: './order-details.html',
  styleUrl: './order-details.css'
})
export class OrderDetailsComponent implements OnInit {

  private readonly orderService = inject(OrderService);
  private readonly addressService = inject(AddressService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private readonly paymentService = inject(PaymentService);

  order: Order | null = null;
  address: Address | null = null;

  isLoading = true;
  errorMessage = '';

  isCreatingPayment = false;
  paymentError = '';

  ngOnInit(): void {

    const idParam =
      this.route.snapshot.paramMap.get('id');

    const orderId = Number(idParam);

    if (
      !idParam ||
      Number.isNaN(orderId) ||
      orderId <= 0
    ) {
      this.errorMessage = 'Invalid order.';
      this.isLoading = false;

      this.changeDetector.detectChanges();

      return;
    }

    this.loadOrder(orderId);
  }

  // ==========================================
  // PROCEED TO ONLINE PAYMENT
  // ==========================================

  proceedToPayment(): void {

    if (
      !this.order ||
      this.isCreatingPayment ||
      this.order.paymentMethod !== 'ONLINE'
    ) {
      return;
    }

    this.isCreatingPayment = true;
    this.paymentError = '';

    this.changeDetector.detectChanges();

    this.paymentService
      .createPayment(this.order.id)
      .subscribe({

        next: (response) => {

          console.log(
            'Razorpay order created:',
            response
          );

          if (
            !response.success ||
            !response.data
          ) {

            this.isCreatingPayment = false;

            this.paymentError =
              response.message ||
              'Unable to create payment.';

            this.changeDetector.detectChanges();

            return;
          }

          this.openRazorpayCheckout(
            response.data
          );
        },

        error: (error) => {

          console.error(
            'Create payment error:',
            error
          );

          this.isCreatingPayment = false;

          this.paymentError =
            error?.error?.message ||
            'Unable to start payment. Please try again.';

          this.changeDetector.detectChanges();
        }

      });
  }

  // ==========================================
  // OPEN RAZORPAY CHECKOUT
  // ==========================================

  private openRazorpayCheckout(
    payment: {
      orderId: number;
      orderNumber: string;
      amount: number;
      razorpayOrderId: string;
      currency: string;
      paymentId: string | null;
      paymentStatus: string;
    }
  ): void {

    if (!window.Razorpay) {

      this.isCreatingPayment = false;

      this.paymentError =
        'Payment gateway could not be loaded. Please refresh and try again.';

      this.changeDetector.detectChanges();

      return;
    }

    const options: RazorpayOptions = {

      key: environment.razorpayKeyId,

      amount: payment.amount * 100,

      currency: payment.currency,

      name: 'ShankarAgriMart',

      description:
        `Payment for ${payment.orderNumber}`,

      order_id:
        payment.razorpayOrderId,

      handler: (
        response: RazorpayResponse
      ) => {

        this.verifyPayment(response);
      },

      theme: {
        color: '#198754'
      },

      modal: {

        ondismiss: () => {

          this.isCreatingPayment = false;

          this.changeDetector.detectChanges();
        }

      }

    };

    const razorpay =
      new window.Razorpay(options);

    razorpay.open();
  }

  // ==========================================
  // VERIFY RAZORPAY PAYMENT
  // ==========================================

  private verifyPayment(
    response: RazorpayResponse
  ): void {

    if (!this.order) {
      return;
    }

    this.paymentService
      .verifyPayment(
        this.order.id,
        {
          razorpayPaymentId:
            response.razorpay_payment_id,

          razorpayOrderId:
            response.razorpay_order_id,

          razorpaySignature:
            response.razorpay_signature
        }
      )
      .subscribe({

        next: (result) => {

          console.log(
            'Payment verification response:',
            result
          );

          this.isCreatingPayment = false;

          if (result.success) {

            this.router.navigate([
              '/orders',
              this.order!.id
            ]);

          } else {

            this.paymentError =
              result.message ||
              'Payment verification failed.';

          }

          this.changeDetector.detectChanges();
        },

        error: (error) => {

          console.error(
            'Payment verification error:',
            error
          );

          this.isCreatingPayment = false;

          this.paymentError =
            error?.error?.message ||
            'Payment verification failed. Please contact support if money was deducted.';

          this.changeDetector.detectChanges();
        }

      });
  }

  // ==========================================
  // LOAD ORDER
  // ==========================================

  private loadOrder(
    orderId: number
  ): void {

    this.orderService
      .getOrderById(orderId)
      .subscribe({

        next: (order: Order) => {

          console.log(
            'Order loaded:',
            order
          );

          this.order = order;

          this.changeDetector.detectChanges();

          this.loadAddress(
            order.addressId
          );
        },

        error: (error) => {

          console.error(
            'Order details API error:',
            error
          );

          this.errorMessage =
            'Unable to load order details. Please try again later.';

          this.isLoading = false;

          this.changeDetector.detectChanges();
        }

      });
  }

  // ==========================================
  // LOAD ADDRESS
  // ==========================================

  private loadAddress(
    addressId: number
  ): void {

    this.addressService
      .getAddressById(addressId)
      .subscribe({

        next: (address: Address) => {

          console.log(
            'Address loaded:',
            address
          );

          this.address = address;

          this.isLoading = false;

          this.changeDetector.detectChanges();
        },

        error: (error) => {

          console.error(
            'Address API error:',
            error
          );

          // Order can still be displayed
          // even if address loading fails.

          this.isLoading = false;

          this.changeDetector.detectChanges();
        }

      });
  }

  // ==========================================
  // PAYMENT STATUS
  // ==========================================

  getPaymentStatus(): string {

    if (!this.order) {
      return '';
    }

    return this.order.paymentStatus === 1
      ? 'Pending'
      : 'Paid';
  }

  // ==========================================
  // PAYMENT METHOD
  // ==========================================

  getPaymentMethod(): string {

    if (!this.order) {
      return '';
    }

    return this.order.paymentMethod === 'COD'
      ? 'Cash on Delivery'
      : 'Online Payment';
  }

  // ==========================================
  // CHECK IF ONLINE PAYMENT
  // ==========================================

  isOnlinePayment(): boolean {

    return this.order?.paymentMethod === 'ONLINE';
  }

  // ==========================================
  // ORDER STATUS
  // ==========================================

  getOrderStatus(): string {

    if (!this.order) {
      return '';
    }

    switch (this.order.orderStatus) {

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
}