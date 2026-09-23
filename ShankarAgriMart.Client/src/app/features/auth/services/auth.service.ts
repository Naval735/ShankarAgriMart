import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from '../../../core/service/api.service';
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest
} from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly api = inject(ApiService);

  login(request: LoginRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>(
      'Auth/login',
      request
    );
  }

  register(request: RegisterRequest): Observable<AuthResponse> {
    return this.api.post<AuthResponse>(
      'Auth/register',
      request
    );
  }

  getProfile<T>(): Observable<T> {
    return this.api.get<T>('User/profile');
  }

logout(): Observable<{ message: string }> {
  return this.api.post<{ message: string }>(
    'Auth/logout',
    {}
  );
}
}