import {
  Injectable,
  inject
} from '@angular/core';

import {
  Observable
} from 'rxjs';

import {
  ApiService
} from '../../../core/service/api.service';

import {
  Brand
} from '../models/brand.model';

export interface BrandApiResponse {
  success: boolean;
  message: string;
  data: Brand[];
}

@Injectable({
  providedIn: 'root'
})
export class BrandService {

  private readonly api =
    inject(ApiService);

  getBrands(): Observable<BrandApiResponse> {

    return this.api.get<BrandApiResponse>(
      'Brand'
    );
  }
}