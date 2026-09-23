import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { Router, RouterLink } from '@angular/router';

import { CartService } from '../../../cart/services/cart.service';
import { Cart } from '../../../cart/models/cart.model';

import {
  Address,
  CreateAddressRequest
} from '../../models/address.model';

import { AddressService } from '../../services/address.service';

import { OrderService } from '../../../orders/services/order.service';

import {
  CreateOrderRequest,
  Order
} from '../../../orders/models/order.model';

@Component({
  selector: 'app-checkout',
  imports: [RouterLink],
  templateUrl: './checkout.html',
  styleUrl: './checkout.css'
})
export class CheckoutComponent implements OnInit {

  private readonly cartService = inject(CartService);
  private readonly addressService = inject(AddressService);
  private readonly orderService = inject(OrderService);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);

  cart: Cart | null = null;

  addresses: Address[] = [];

  selectedAddressId: number | null = null;

  // Selected payment method
  selectedPaymentMethod: 'ONLINE' | 'COD' = 'ONLINE';

  isLoading = true;
  isPlacingOrder = false;

  errorMessage = '';

  showAddressForm = false;

  ngOnInit(): void {
    this.loadCheckoutData();
  }

  private loadCheckoutData(): void {
    this.isLoading = true;

    this.cartService.getCart().subscribe({
      next: (cart: Cart) => {
        this.cart = cart;

        if (cart.items.length === 0) {
          this.router.navigate(['/cart']);
          return;
        }

        this.loadAddresses();
      },

      error: (error) => {
        console.error(
          'Cart checkout error:',
          error
        );

        this.errorMessage =
          'Unable to load your cart. Please try again later.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }
    });
  }

  private loadAddresses(): void {
    this.addressService.getAddresses().subscribe({
      next: (addresses: Address[]) => {
        this.addresses = addresses;

        const defaultAddress = addresses.find(
          address => address.isDefault
        );

        this.selectedAddressId =
          defaultAddress?.id ??
          addresses[0]?.id ??
          null;

        this.isLoading = false;

        this.changeDetector.detectChanges();
      },

      error: (error) => {
        console.error(
          'Address API error:',
          error
        );

        this.errorMessage =
          'Unable to load your delivery addresses.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }
    });
  }

  selectAddress(addressId: number): void {
    this.selectedAddressId = addressId;
  }

  selectPaymentMethod(
    paymentMethod: 'ONLINE' | 'COD'
  ): void {
    this.selectedPaymentMethod = paymentMethod;
  }

  placeOrder(): void {

    if (!this.selectedAddressId) {
      this.errorMessage =
        'Please select a delivery address.';

      this.changeDetector.detectChanges();

      return;
    }

    this.isPlacingOrder = true;
    this.errorMessage = '';

    const request: CreateOrderRequest = {
      addressId: this.selectedAddressId,
      paymentMethod: this.selectedPaymentMethod
    };

    console.log(
      'Creating order with payment method:',
      this.selectedPaymentMethod
    );

    this.orderService
      .createOrder(request)
      .subscribe({
        next: (order: Order) => {

          console.log(
            'Order created:',
            order
          );

          this.isPlacingOrder = false;

          this.changeDetector.detectChanges();

          /*
           * Both payment methods go to Order Details.
           *
           * COD:
           * Order is already created with COD.
           *
           * ONLINE:
           * Customer can continue to Razorpay
           * from the Order Details page.
           */
          this.router.navigate([
            '/orders',
            order.id
          ]);
        },

        error: (error) => {
          console.error(
            'Create order error:',
            error
          );

          this.isPlacingOrder = false;

          this.errorMessage =
            error?.error?.message ||
            'Unable to place your order. Please try again.';

          this.changeDetector.detectChanges();
        }
      });
  }

  getTotalQuantity(): number {
    return this.cart?.items.reduce(
      (total, item) =>
        total + item.quantity,
      0
    ) ?? 0;
  }
}