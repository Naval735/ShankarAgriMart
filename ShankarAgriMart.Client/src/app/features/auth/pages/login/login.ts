import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import {
  AuthService,
  AuthUser
} from '../../services/auth.service';

interface UserProfileResponse {
  success: boolean;
  data: AuthUser;
}

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.html',
  styleUrl: './login.css'
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isLoading = false;
  errorMessage = '';

  loginForm = this.fb.nonNullable.group({
    email: ['', [
      Validators.required,
      Validators.email
    ]],
    password: ['', [
      Validators.required,
      Validators.minLength(6)
    ]]
  });

  get email() {
    return this.loginForm.controls.email;
  }

  get password() {
    return this.loginForm.controls.password;
  }

  onSubmit(): void {
    this.errorMessage = '';

    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;

    this.authService.login(this.loginForm.getRawValue())
      .subscribe({
        next: () => {
          this.authService.getProfile<UserProfileResponse>()
            .subscribe({
              next: (response) => {
                if (response?.success && response?.data) {
                  this.authService.setCurrentUser(response.data);
                  this.isLoading = false;
                  this.router.navigate(['/home']);
                } else {
                  this.isLoading = false;
                  this.errorMessage =
                    'Login succeeded, but user details could not be loaded. Please try again.';
                }
              },
              error: (error) => {
                console.error('Profile loading failed:', error);
                this.isLoading = false;
                this.errorMessage =
                  'Login succeeded, but user details could not be loaded. Please try again.';
              }
            });
        },
        error: (error) => {
          console.error('Login failed:', error);
          this.isLoading = false;
          this.errorMessage =
            error?.error?.message ??
            'Unable to login. Please check your email and password.';
        }
      });
  }
}