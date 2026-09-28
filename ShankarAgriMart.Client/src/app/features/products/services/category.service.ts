
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import { ApiResponse } from '../models/api-response.model';
import { Category } from '../models/category.model';

export interface CreateCategoryRequest {
  name: string;
  description?: string | null;
  imageUrl?: string | null;
  displayOrder: number;
}

export interface UpdateCategoryRequest {
  name: string;
  description?: string | null;
  imageUrl?: string | null;
  displayOrder: number;
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private readonly api = inject(ApiService);
  private readonly endpoint = 'Category';

  // Get all categories
  getCategories(): Observable<ApiResponse<Category[]>> {
    return this.api.get<ApiResponse<Category[]>>(
      this.endpoint
    );
  }

  // Get category by ID
  getCategoryById(id: number): Observable<ApiResponse<Category>> {
    return this.api.get<ApiResponse<Category>>(
      `${this.endpoint}/${id}`
    );
  }

  // Create category
  createCategory(
    request: CreateCategoryRequest
  ): Observable<ApiResponse<Category>> {
    return this.api.post<ApiResponse<Category>>(
      this.endpoint,
      request
    );
  }

  // Update category
  updateCategory(
    id: number,
    request: UpdateCategoryRequest
  ): Observable<ApiResponse<Category>> {
    return this.api.put<ApiResponse<Category>>(
      `${this.endpoint}/${id}`,
      request
    );
  }

  // Delete category
  deleteCategory(id: number): Observable<ApiResponse<unknown>> {
    return this.api.delete<ApiResponse<unknown>>(
      `${this.endpoint}/${id}`
    );
  }
}