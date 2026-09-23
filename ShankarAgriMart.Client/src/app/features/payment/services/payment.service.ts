import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';

import {
  CreatePaymentResponse,
  VerifyPaymentRequest,
  VerifyPaymentResponse
} from '../models/payment.model';

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private readonly api = inject(ApiService);

  createPayment(
    orderId: number
  ): Observable<CreatePaymentResponse> {
    return this.api.post<CreatePaymentResponse>(
      `Payment/create/${orderId}`,
      {}
    );
  }

  verifyPayment(
    orderId: number,
    request: VerifyPaymentRequest
  ): Observable<VerifyPaymentResponse> {
    return this.api.post<VerifyPaymentResponse>(
      `Payment/verify/${orderId}`,
      request
    );
  }
}