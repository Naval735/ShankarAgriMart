import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import {
  Address,
  CreateAddressRequest
} from '../models/address.model';

@Injectable({
  providedIn: 'root'
})
export class AddressService {
  private readonly api = inject(ApiService);

  getAddresses(): Observable<Address[]> {
    return this.api.get<Address[]>('Address');
  }

  getAddressById(id: number): Observable<Address> {
    return this.api.get<Address>(`Address/${id}`);
  }

  createAddress(
    request: CreateAddressRequest
  ): Observable<Address> {
    return this.api.post<Address>('Address', request);
  }
}