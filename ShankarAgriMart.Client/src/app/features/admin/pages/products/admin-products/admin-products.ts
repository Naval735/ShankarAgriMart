import {Component,OnInit,inject, ChangeDetectorRef} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  Router,
  RouterLink
} from '@angular/router';

import {
  FormsModule
} from '@angular/forms';

import {
  AdminProductService
} from '../../../services/admin-product.service';

import {
  Product
} from '../../../../products/models/product.model';

@Component({
  selector: 'app-admin-products',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    RouterLink
  ],
  templateUrl: './admin-products.html',
  styleUrl: './admin-products.css'
})
export class AdminProductsComponent implements OnInit {

  private readonly productService =
    inject(AdminProductService);
private readonly changeDetector =
  inject(ChangeDetectorRef);
  private readonly router =
    inject(Router);

  products: Product[] = [];
  filteredProducts: Product[] = [];

  searchTerm = '';

  isLoading = true;
  isDeleting = false;

  errorMessage = '';

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {

  this.isLoading = true;
  this.errorMessage = '';

  this.changeDetector.detectChanges();

  this.productService
    .getProducts()
    .subscribe({

      next: (products) => {

        this.products = products ?? [];

        this.filteredProducts = [
          ...this.products
        ];

        this.isLoading = false;

        this.changeDetector.detectChanges();
      },

      error: (error) => {

        console.error(
          'Failed to load products:',
          error
        );

        this.products = [];
        this.filteredProducts = [];

        this.errorMessage =
          'Unable to load products. Please try again.';

        this.isLoading = false;

        this.changeDetector.detectChanges();
      }

    });
}
  onSearch(): void {

    const search =
      this.searchTerm
        .trim()
        .toLowerCase();

    if (!search) {

      this.filteredProducts = [
        ...this.products
      ];

      return;
    }

    this.filteredProducts =
      this.products.filter(product =>
        product.name
          .toLowerCase()
          .includes(search)
        ||
        product.sku
          .toLowerCase()
          .includes(search)
        ||
        product.categoryName
          .toLowerCase()
          .includes(search)
        ||
        product.brandName
          .toLowerCase()
          .includes(search)
      );
  }

  clearSearch(): void {

    this.searchTerm = '';

    this.filteredProducts = [
      ...this.products
    ];
  }

  editProduct(product: Product): void {

    this.router.navigate([
      '/admin/products/edit',
      product.id
    ]);
  }

  deleteProduct(product: Product): void {

    const confirmed =
      window.confirm(
        `Are you sure you want to delete "${product.name}"?`
      );

    if (!confirmed) {
      return;
    }

    this.isDeleting = true;

    this.productService
      .deleteProduct(product.id)
      .subscribe({

        next: () => {

          this.products =
            this.products.filter(
              item => item.id !== product.id
            );

          this.filteredProducts =
            this.filteredProducts.filter(
              item => item.id !== product.id
            );

          this.isDeleting = false;
        },

        error: (error) => {

          console.error(
            'Failed to delete product:',
            error
          );

          this.isDeleting = false;

          this.errorMessage =
            'Unable to delete the product. Please try again.';
        }

      });
  }

  getStockClass(stock: number): string {

    if (stock <= 0) {
      return 'text-danger';
    }

    if (stock <= 10) {
      return 'text-warning';
    }

    return 'text-success';
  }
}