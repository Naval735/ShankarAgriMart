import { Injectable, inject } from '@angular/core';
import { BehaviorSubject, Observable, tap } from 'rxjs';

import { ApiService } from '../../../core/service/api.service';
import {
  AuthResponse,
  LoginRequest,
  RegisterRequest
} from '../models/auth.models';

export interface AuthUser {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  emailVerified: boolean;
  role: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly api = inject(ApiService);

  private readonly currentUserSubject =
    new BehaviorSubject<AuthUser | null>(null);

  readonly currentUser$ = this.currentUserSubject.asObservable();

  get currentUser(): AuthUser | null {
    return this.currentUserSubject.value;
  }

  setCurrentUser(user: AuthUser | null): void {
    this.currentUserSubject.next(user);
  }

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
    ).pipe(
      tap(() => this.setCurrentUser(null))
    );
  }
}