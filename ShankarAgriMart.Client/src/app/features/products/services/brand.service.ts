import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import { ApiResponse } from '../models/api-response.model';
import { Brand } from '../models/brand.model';

export interface CreateBrandRequest {
  name: string;
  logoUrl: string | null;
  description: string | null;
}

export interface UpdateBrandRequest extends CreateBrandRequest {
  isActive: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class BrandService {
  private readonly api = inject(ApiService);
  private readonly endpoint = 'Brand';

  getBrands(): Observable<ApiResponse<Brand[]>> {
    return this.api.get<ApiResponse<Brand[]>>(this.endpoint);
  }

  getBrandById(id: number): Observable<ApiResponse<Brand>> {
    return this.api.get<ApiResponse<Brand>>(`${this.endpoint}/${id}`);
  }

  createBrand(
    request: CreateBrandRequest
  ): Observable<ApiResponse<Brand>> {
    return this.api.post<ApiResponse<Brand>>(
      this.endpoint,
      request
    );
  }

  updateBrand(
    id: number,
    request: UpdateBrandRequest
  ): Observable<ApiResponse<Brand>> {
    return this.api.put<ApiResponse<Brand>>(
      `${this.endpoint}/${id}`,
      request
    );
  }

  deleteBrand(id: number): Observable<ApiResponse<null>> {
    return this.api.delete<ApiResponse<null>>(
      `${this.endpoint}/${id}`
    );
  }
}