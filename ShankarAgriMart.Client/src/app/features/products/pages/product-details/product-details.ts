import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';

import { DatePipe } from '@angular/common';
import { ActivatedRoute, RouterLink, Router } from '@angular/router';
import { CartService } from '../../../cart/services/cart.service';
import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-product-details',
  imports: [RouterLink, DatePipe],
  templateUrl: './product-details.html',
  styleUrl: './product-details.css'
})
export class ProductDetailsComponent implements OnInit {
  private readonly cartService = inject(CartService);
  private readonly productService = inject(ProductService);
  private readonly route = inject(ActivatedRoute);
  private readonly changeDetector = inject(ChangeDetectorRef);
  private readonly router = inject(Router);
  product: Product | null = null;

  isLoading = true;
  errorMessage = '';
  quantity = 1;
isAddingToCart = false;
cartMessage = '';
cartError = '';
  ngOnInit(): void {

    const idParam =
      this.route.snapshot.paramMap.get('id');

    const productId = Number(idParam);

    if (!idParam || Number.isNaN(productId) || productId <= 0) {

      this.errorMessage = 'Invalid product.';
      this.isLoading = false;

      this.changeDetector.detectChanges();

      return;
    }

    this.loadProduct(productId);
  }
  

   addToCart(): void {
  if (!this.product || this.product.stock <= 0) {
    return;
  }

  this.isAddingToCart = true;
  this.cartMessage = '';
  this.cartError = '';

  this.changeDetector.detectChanges();

  this.cartService
    .addToCart({
      productId: this.product.id,
      quantity: this.quantity
    })
    .subscribe({
     next: (cart) => {
  console.log('Cart updated:', cart);

  this.isAddingToCart = false;
  this.cartMessage = 'Product added to cart successfully.';
  this.cartError = '';

  this.changeDetector.detectChanges();

  setTimeout(() => {
    this.router.navigate(['/cart']);
  }, 100);
},
      error: (error) => {
        console.error('Add to cart error:', error);

        this.isAddingToCart = false;
        this.cartMessage = '';
        this.cartError =
          error?.error?.message ||
          'Unable to add product to cart. Please try again.';

        this.changeDetector.detectChanges();
      }
    });
}
  private loadProduct(id: number): void {

    this.productService.getProductById(id).subscribe({

      next: (product: Product) => {

        this.product = product;
        this.isLoading = false;

        this.changeDetector.detectChanges();
      },

      error: (error) => {

        console.error(
          'Product details API error:',
          error
        );

        this.errorMessage =
          'Unable to load product details. Please try again later.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }

    });
  }
}