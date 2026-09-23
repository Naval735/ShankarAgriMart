import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import {
  AddToCartRequest,
  Cart
} from '../models/cart.model';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private readonly api = inject(ApiService);

  getCart(): Observable<Cart> {
    return this.api.get<Cart>('Cart');
  }

  addToCart(request: AddToCartRequest): Observable<Cart> {
    return this.api.post<Cart>('Cart/items', request);
  }

  updateCartItem(
    cartItemId: number,
    quantity: number
  ): Observable<Cart> {
    return this.api.put<Cart>(
      `Cart/items/${cartItemId}`,
      quantity
    );
  }

  removeCartItem(cartItemId: number): Observable<Cart> {
    return this.api.delete<Cart>(
      `Cart/items/${cartItemId}`
    );
  }
}