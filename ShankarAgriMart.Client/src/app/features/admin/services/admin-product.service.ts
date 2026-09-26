import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';

import { Product } from '../../products/models/product.model';

import { CreateProductRequest } from '../../products/models/product-request.model';

interface ProductApiResponse {
  success: boolean;
  data: Product[];
  message?: string;
}

@Injectable({
  providedIn: 'root'
})
export class AdminProductService {

  private readonly api = inject(ApiService);

  getProducts(): Observable<Product[]> {

    return this.api
      .get<Product[] | ProductApiResponse>('Product')
      .pipe(
        map(response => {

          if (Array.isArray(response)) {
            return response;
          }

          return response.data ?? [];

        })
      );
  }

  getProductById(
    id: number
  ): Observable<Product> {

    return this.api.get<Product>(
      `Product/${id}`
    );
  }

  createProduct(
    request: CreateProductRequest
  ): Observable<Product> {

    return this.api.post<Product>(
      'Product',
      request
    );
  }

  deleteProduct(
    id: number
  ): Observable<void> {

    return this.api.delete<void>(
      `Product/${id}`
    );
  }
}