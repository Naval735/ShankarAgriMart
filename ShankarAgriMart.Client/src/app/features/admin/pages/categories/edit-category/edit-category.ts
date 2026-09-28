
import { CommonModule } from '@angular/common';
import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';
import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import {
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';

import {
  CategoryService,
  UpdateCategoryRequest
} from '../../../../products/services/category.service';

@Component({
  selector: 'app-edit-category',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './edit-category.html',
  styleUrl: './edit-category.css'
})
export class EditCategoryComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly categoryService = inject(CategoryService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly changeDetectorRef = inject(ChangeDetectorRef);

  categoryId = 0;
  isLoading = true;
  isSubmitting = false;
  categoryLoaded = false;
  errorMessage = '';

  categoryForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    description: ['', [Validators.maxLength(500)]],
    imageUrl: [''],
    displayOrder: [0, [Validators.required, Validators.min(0)]],
    isActive: [true]
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

  get isActive() {
    return this.categoryForm.controls.isActive;
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!Number.isInteger(id) || id <= 0) {
      this.errorMessage = 'Invalid category ID.';
      this.isLoading = false;
      return;
    }

    this.categoryId = id;
    this.loadCategory();
  }

  loadCategory(): void {
    if (this.categoryId <= 0) {
      this.errorMessage = 'Invalid category ID.';
      this.isLoading = false;
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.categoryLoaded = false;

    this.changeDetectorRef.detectChanges();

    this.categoryService.getCategoryById(this.categoryId).subscribe({
      next: (response) => {
        console.log('Category API response:', response);

        if (response.success && response.data) {
          const category = response.data;

          this.categoryForm.patchValue({
            name: category.name ?? '',
            description: category.description ?? '',
            imageUrl: category.imageUrl ?? '',
            displayOrder: category.displayOrder ?? 0,
            isActive: category.isActive ?? false
          });

          this.categoryLoaded = true;
          this.errorMessage = '';
        } else {
          this.categoryLoaded = false;
          this.errorMessage =
            response.message || 'Unable to load category.';
        }

        this.isLoading = false;
        this.changeDetectorRef.detectChanges();
      },

      error: (error) => {
        console.error('Failed to load category:', error);

        this.categoryLoaded = false;
        this.errorMessage =
          error?.error?.message ||
          'Unable to load category. Please try again.';

        this.isLoading = false;
        this.changeDetectorRef.detectChanges();
      }
    });
  }

  onSubmit(): void {
    this.errorMessage = '';

    if (this.categoryForm.invalid) {
      this.categoryForm.markAllAsTouched();
      return;
    }

    if (!this.categoryLoaded || this.categoryId <= 0) {
      this.errorMessage = 'Category details are not loaded.';
      return;
    }

    const formValue = this.categoryForm.getRawValue();

    const request: UpdateCategoryRequest = {
      name: (formValue.name ?? '').trim(),
      description: formValue.description?.trim() || null,
      imageUrl: formValue.imageUrl?.trim() || null,
      displayOrder: Number(formValue.displayOrder ?? 0),
      isActive: formValue.isActive ?? false
    };

    this.isSubmitting = true;
    this.changeDetectorRef.detectChanges();

    this.categoryService
      .updateCategory(this.categoryId, request)
      .subscribe({
        next: (response) => {
          this.isSubmitting = false;

          if (response.success) {
            void this.router.navigate(['/admin/categories']);
          } else {
            this.errorMessage =
              response.message || 'Unable to update category.';
          }

          this.changeDetectorRef.detectChanges();
        },

        error: (error) => {
          console.error('Failed to update category:', error);

          this.isSubmitting = false;
          this.errorMessage =
            error?.error?.message ||
            'Unable to update category. Please try again.';

          this.changeDetectorRef.detectChanges();
        }
      });
  }

  onCancel(): void {
    if (!this.isSubmitting) {
      void this.router.navigate(['/admin/categories']);
    }
  }
}