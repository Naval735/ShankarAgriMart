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
  BrandService,
  UpdateBrandRequest
} from '../../../../products/services/brand.service';

@Component({
  selector: 'app-edit-brand',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './edit-brand.html',
  styleUrl: './edit-brand.css'
})
export class EditBrandComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly brandService = inject(BrandService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly changeDetector = inject(ChangeDetectorRef);

  brandId = 0;
  isLoading = true;
  isSubmitting = false;
  brandLoaded = false;
  errorMessage = '';

  brandForm = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    logoUrl: [''],
    description: ['', [Validators.maxLength(500)]],
    isActive: [true]
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

  get isActive() {
    return this.brandForm.controls.isActive;
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if (!Number.isInteger(id) || id <= 0) {
      this.errorMessage = 'Invalid brand ID.';
      this.isLoading = false;
      return;
    }

    this.brandId = id;
    this.loadBrand();
  }

  loadBrand(): void {
    if (this.brandId <= 0) {
      this.errorMessage = 'Invalid brand ID.';
      this.isLoading = false;
      return;
    }

    this.isLoading = true;
    this.errorMessage = '';
    this.brandLoaded = false;
    this.changeDetector.detectChanges();

    this.brandService.getBrandById(this.brandId).subscribe({
      next: (response) => {
        if (response.success && response.data) {
          const brand = response.data;

          this.brandForm.patchValue({
            name: brand.name ?? '',
            logoUrl: brand.logoUrl ?? '',
            description: brand.description ?? '',
            isActive: brand.isActive ?? false
          });

          this.brandLoaded = true;
          this.errorMessage = '';
        } else {
          this.errorMessage =
            response.message || 'Unable to load brand.';
        }

        this.isLoading = false;
        this.changeDetector.detectChanges();
      },
      error: (error) => {
        console.error('Failed to load brand:', error);

        this.brandLoaded = false;
        this.errorMessage =
          error?.error?.message ||
          'Unable to load brand. Please try again.';

        this.isLoading = false;
        this.changeDetector.detectChanges();
      }
    });
  }

  onSubmit(): void {
    this.errorMessage = '';

    if (this.brandForm.invalid) {
      this.brandForm.markAllAsTouched();
      return;
    }

    if (!this.brandLoaded || this.brandId <= 0) {
      this.errorMessage = 'Brand details are not loaded.';
      return;
    }

    const formValue = this.brandForm.getRawValue();

    const request: UpdateBrandRequest = {
      name: (formValue.name ?? '').trim(),
      logoUrl: formValue.logoUrl?.trim() || null,
      description: formValue.description?.trim() || null,
      isActive: formValue.isActive ?? false
    };

    this.isSubmitting = true;
    this.changeDetector.detectChanges();

    this.brandService.updateBrand(this.brandId, request).subscribe({
      next: (response) => {
        this.isSubmitting = false;

        if (response.success) {
          void this.router.navigate(['/admin/brands']);
        } else {
          this.errorMessage =
            response.message || 'Unable to update brand.';
        }

        this.changeDetector.detectChanges();
      },
      error: (error) => {
        console.error('Failed to update brand:', error);

        this.isSubmitting = false;
        this.errorMessage =
          error?.error?.message ||
          'Unable to update brand. Please try again.';

        this.changeDetector.detectChanges();
      }
    });
  }

  onCancel(): void {
    if (!this.isSubmitting) {
      void this.router.navigate(['/admin/brands']);
    }
  }
}