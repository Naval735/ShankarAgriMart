import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';

import { ProductService } from '../../services/product.service';
import { Product } from '../../models/product.model';

@Component({
  selector: 'app-products',
  imports: [RouterLink],
  templateUrl: './products.html',
  styleUrl: './products.css'
})
export class ProductsComponent implements OnInit {

  private readonly productService = inject(ProductService);
  private readonly route = inject(ActivatedRoute);
  private readonly changeDetector = inject(ChangeDetectorRef);

  products: Product[] = [];

  isLoading = true;
  errorMessage = '';

  categoryId: number | null = null;

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
}