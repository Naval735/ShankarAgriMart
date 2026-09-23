import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import { Product } from '../models/product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  private readonly api = inject(ApiService);

  getProducts(): Observable<Product[]> {
    return this.api.get<Product[]>('Product');
  }

  getProductById(id: number): Observable<Product> {
    return this.api.get<Product>(`Product/${id}`);
  }
}