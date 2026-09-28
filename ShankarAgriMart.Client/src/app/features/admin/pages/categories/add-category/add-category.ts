
import { CommonModule } from '@angular/common';
import { Component, inject } from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';

import {
  CategoryService,
  CreateCategoryRequest
} from '../../../../products/services/category.service';

@Component({
  selector: 'app-add-category',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './add-category.html',
  styleUrl: './add-category.css'
})
export class AddCategoryComponent {
  private readonly fb = inject(FormBuilder);
  private readonly categoryService = inject(CategoryService);
  private readonly router = inject(Router);

  isSubmitting = false;
  errorMessage = '';

  categoryForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(500)]],
    imageUrl: [''],
    displayOrder: [0, [Validators.required, Validators.min(0)]]
  });

  get name() {
    return this.categoryForm.controls.name;
  }

  get description() {
    return this.categoryForm.controls.description;
  }

  get imageUrl() {
    return this.categoryForm.controls.imageUrl;
  }

  get displayOrder() {
    return this.categoryForm.controls.displayOrder;
  }

  onSubmit(): void {
    this.errorMessage = '';

    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    const formValue = this.categoryForm.getRawValue();

    const request: CreateCategoryRequest = {
  name: (formValue.name ?? '').trim(),
  description: formValue.description?.trim() || null,
  imageUrl: formValue.imageUrl?.trim() || null,
  displayOrder: Number(formValue.displayOrder ?? 0)
};

    this.isSubmitting = true;

    this.categoryService.createCategory(request).subscribe({
      next: (response) => {
        this.isSubmitting = false;

        if (response.success) {
          void this.router.navigate(['/admin/categories']);
        } else {
          this.errorMessage =
            response.message || 'Unable to create category.';
        }
      },
      error: (error) => {
        console.error('Failed to create category:', error);

        this.isSubmitting = false;
        this.errorMessage =
          error?.error?.message ||
          'Unable to create category. Please try again.';
      }
    });
  }

  onCancel(): void {
    void this.router.navigate(['/admin/categories']);
  }
}