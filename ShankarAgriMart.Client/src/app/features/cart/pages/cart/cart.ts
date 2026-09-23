import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';
import { RouterLink } from '@angular/router';

import { CartService } from '../../services/cart.service';
import { Cart } from '../../models/cart.model';

@Component({
  selector: 'app-cart',
  imports: [RouterLink],
  templateUrl: './cart.html',
  styleUrl: './cart.css'
})
export class CartComponent implements OnInit {
  private readonly cartService = inject(CartService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  cart: Cart | null = null;

  isLoading = true;
  errorMessage = '';

  updatingItemId: number | null = null;
  removingItemId: number | null = null;

  ngOnInit(): void {
    this.loadCart();
  }

  private loadCart(): void {
    this.cartService.getCart().subscribe({
      next: (cart: Cart) => {
        this.cart = cart;
        this.isLoading = false;

        this.changeDetector.detectChanges();
      },
      error: (error) => {
        console.error('Cart API error:', error);

        this.errorMessage =
          'Unable to load your cart. Please try again later.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }
    });
  }

  increaseQuantity(itemId: number, currentQuantity: number): void {
    this.updateQuantity(itemId, currentQuantity + 1);
  }

  decreaseQuantity(itemId: number, currentQuantity: number): void {
    if (currentQuantity <= 1) {
      return;
    }

    this.updateQuantity(itemId, currentQuantity - 1);
  }

  private updateQuantity(
    itemId: number,
    quantity: number
  ): void {
    this.updatingItemId = itemId;

    this.changeDetector.detectChanges();

    this.cartService
      .updateCartItem(itemId, quantity)
      .subscribe({
        next: (cart: Cart) => {
          this.cart = cart;
          this.updatingItemId = null;

          this.changeDetector.detectChanges();
        },
        error: (error) => {
          console.error('Update cart item error:', error);

          this.updatingItemId = null;

          this.changeDetector.detectChanges();
        }
      });
  }

  removeItem(itemId: number): void {
    this.removingItemId = itemId;

    this.changeDetector.detectChanges();

    this.cartService
      .removeCartItem(itemId)
      .subscribe({
        next: (cart: Cart) => {
          this.cart = cart;
          this.removingItemId = null;

          this.changeDetector.detectChanges();
        },
        error: (error) => {
          console.error('Remove cart item error:', error);

          this.removingItemId = null;

          this.changeDetector.detectChanges();
        }
      });
  }

  getTotalQuantity(): number {
    return this.cart?.items.reduce(
      (total, item) => total + item.quantity,
      0
    ) ?? 0;
  }
}