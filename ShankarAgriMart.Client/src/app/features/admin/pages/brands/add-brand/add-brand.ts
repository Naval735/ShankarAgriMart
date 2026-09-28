import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import {
  BrandService,
  CreateBrandRequest
} from '../../../../products/services/brand.service';

@Component({
  selector: 'app-add-brand',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './add-brand.html',
  styleUrl: './add-brand.css'
})
export class AddBrandComponent {
  private readonly fb = inject(FormBuilder);
  private readonly brandService = inject(BrandService);
  private readonly router = inject(Router);

  isSubmitting = false;
  errorMessage = '';

  brandForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    logoUrl: [''],
    description: ['', [Validators.maxLength(500)]]
  });

  get name() {
    return this.brandForm.controls.name;
  }

  get logoUrl() {
    return this.brandForm.controls.logoUrl;
  }

  get description() {
    return this.brandForm.controls.description;
  }

  onSubmit(): void {
    this.errorMessage = '';

    if (this.brandForm.invalid) {
      this.brandForm.markAllAsTouched();
      return;
    }

    const formValue = this.brandForm.getRawValue();

    const request: CreateBrandRequest = {
      name: (formValue.name ?? '').trim(),
      logoUrl: formValue.logoUrl?.trim() || null,
      description: formValue.description?.trim() || null
    };

    this.isSubmitting = true;

    this.brandService.createBrand(request).subscribe({
      next: (response) => {
        this.isSubmitting = false;

        if (response.success) {
          void this.router.navigate(['/admin/brands']);
        } else {
          this.errorMessage =
            response.message || 'Unable to create brand.';
        }
      },
      error: (error) => {
        console.error('Failed to create brand:', error);

        this.isSubmitting = false;
        this.errorMessage =
          error?.error?.message ||
          'Unable to create brand. Please try again.';
      }
    });
  }

  onCancel(): void {
    if (!this.isSubmitting) {
      void this.router.navigate(['/admin/brands']);
    }
  }
}