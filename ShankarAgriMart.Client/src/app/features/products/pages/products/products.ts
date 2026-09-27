import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';

import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

import { CartService } from '../../../cart/services/cart.service';
import { AddToCartRequest } from '../../../cart/models/cart.model';


@Component({
  selector: 'app-products',
  imports: [RouterLink],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class ProductsComponent implements OnInit {

  private readonly router = inject(Router);
  private readonly productService = inject(ProductService);
  private readonly cartService = inject(CartService);
  private readonly route = inject(ActivatedRoute);
  private readonly changeDetector = inject(ChangeDetectorRef);


  products: Product[] = [];

  isLoading = true;
  errorMessage = '';

  categoryId: number | null = null;

  addingToCartProductId: number | null = null;

  cartSuccessMessage = '';
  cartErrorMessage = '';


  ngOnInit(): void {

    const categoryIdParam =
      this.route.snapshot.queryParamMap.get('categoryId');

    this.categoryId = categoryIdParam
      ? Number(categoryIdParam)
      : null;

    this.loadProducts();
  }


 
  private loadProducts(): void {

    this.productService.getProducts().subscribe({

      next: (products: Product[]) => {

        if (this.categoryId !== null) {

          this.products = products.filter(
            product => product.categoryId === this.categoryId
          );

        } else {

          this.products = products;

        }

        this.isLoading = false;

        this.changeDetector.detectChanges();
      },


      error: (error) => {

        console.error('Product API error:', error);

        this.errorMessage =
          'Unable to load products. Please try again later.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }

    });
  }



  addToCart(product: Product): void {

    if (this.addingToCartProductId !== null) {
      return;
    }


    this.cartSuccessMessage = '';
    this.cartErrorMessage = '';


    this.addingToCartProductId = product.id;


    const request: AddToCartRequest = {
      productId: product.id,
      quantity: 1
    };


    this.cartService.addToCart(request).subscribe({

      next: () => {

        this.addingToCartProductId = null;

        this.cartSuccessMessage =
          `${product.name} added to cart successfully.`;

        this.changeDetector.detectChanges();
      },


      error: (error) => {

        console.error('Add to cart failed:', error);

        this.addingToCartProductId = null;

        this.cartErrorMessage =
          'Unable to add product to cart. Please try again.';

        this.changeDetector.detectChanges();
      }

    });
  }



  viewCart(): void {

    this.router.navigate(['/cart']);

  }

}