import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';

import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/auth.models';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.html',
  styleUrl: './register.css'
})
export class RegisterComponent {

  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isSubmitting = false;
  errorMessage = '';
  successMessage = '';

  registerForm = this.fb.nonNullable.group({
    firstName: [
      '',
      [
        Validators.required,
        Validators.minLength(2),
        Validators.maxLength(100)
      ]
    ],

    lastName: [
      '',
      [
        Validators.maxLength(100)
      ]
    ],

    email: [
      '',
      [
        Validators.required,
        Validators.email
      ]
    ],

    phone: [
      '',
      [
        Validators.required,
        Validators.pattern(/^[6-9]\d{9}$/)
      ]
    ],

    password: [
      '',
      [
        Validators.required,
        Validators.minLength(8)
      ]
    ],

    confirmPassword: [
      '',
      [
        Validators.required
      ]
    ]
  });

  get firstName() {
    return this.registerForm.controls.firstName;
  }

  get lastName() {
    return this.registerForm.controls.lastName;
  }

  get email() {
    return this.registerForm.controls.email;
  }

  get phone() {
    return this.registerForm.controls.phone;
  }

  get password() {
    return this.registerForm.controls.password;
  }

  get confirmPassword() {
    return this.registerForm.controls.confirmPassword;
  }

  passwordsMatch(): boolean {
    return (
      this.password.value ===
      this.confirmPassword.value
    );
  }

  submit(): void {

    this.errorMessage = '';
    this.successMessage = '';

    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      return;
    }

    if (!this.passwordsMatch()) {
      this.errorMessage =
        'Passwords do not match.';
      return;
    }

    this.isSubmitting = true;

    const request: RegisterRequest = {
      firstName: this.firstName.value.trim(),
      lastName: this.lastName.value.trim(),
      email: this.email.value.trim(),
      phone: this.phone.value.trim(),
      password: this.password.value
    };

    this.authService.register(request).subscribe({

      next: () => {

        this.isSubmitting = false;

        this.successMessage =
          'Registration successful. Please login to continue.';

        this.registerForm.reset();

        setTimeout(() => {
          this.router.navigate(['/login']);
        }, 1200);
      },

      error: (error) => {

        this.isSubmitting = false;

        console.error(
          'Registration failed:',
          error
        );

        this.errorMessage =
          error?.error?.message ||
          'Registration failed. Please try again.';
      }

    });
  }
}