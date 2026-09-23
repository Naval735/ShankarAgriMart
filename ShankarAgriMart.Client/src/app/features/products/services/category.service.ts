import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';

import { ApiResponse } from '../models/api-response.model';
import { Category } from '../models/category.model';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {

  private readonly api = inject(ApiService);

  getCategories(): Observable<ApiResponse<Category[]>> {
    return this.api.get<ApiResponse<Category[]>>(
      'Category'
    );
  }
}