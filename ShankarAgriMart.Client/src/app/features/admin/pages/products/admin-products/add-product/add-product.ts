import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import {
  CommonModule
} from '@angular/common';

import {
  FormBuilder,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import {
  Router,
  RouterLink
} from '@angular/router';

import {
  BrandService
} from '../../../../../products/services/brand.service';

import {
  Brand
} from '../../../../../products/models/brand.model';

import {
  AdminProductService
} from '../../../../services/admin-product.service';

import {
  CategoryService
} from '../../../../../products/services/category.service';

import {
  CreateProductRequest
} from '../../../../../products/models/product-request.model';

import {
  Category
} from '../../../../../products/models/category.model';

@Component({
  selector: 'app-add-product',
  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],

  templateUrl: './add-product.html',
  styleUrl: './add-product.css'
})
export class AddProductComponent implements OnInit {

  private readonly fb =
    inject(FormBuilder);

  private readonly changeDetector =
    inject(ChangeDetectorRef);

  private readonly productService =
    inject(AdminProductService);

  private readonly categoryService =
    inject(CategoryService);

  private readonly brandService =
    inject(BrandService);

  private readonly router =
    inject(Router);

  productForm!: FormGroup;

  categories: Category[] = [];

  brands: Brand[] = [];

  isLoading = true;
  isSubmitting = false;

  errorMessage = '';

  ngOnInit(): void {
    this.createForm();
    this.loadDropdownData();
  }

  private createForm(): void {

    this.productForm = this.fb.group({

      categoryId: [
        '',
        Validators.required
      ],

      brandId: [
        '',
        Validators.required
      ],

      name: [
        '',
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(200)
        ]
      ],

      shortDescription: [
        '',
        Validators.maxLength(500)
      ],

      description: [
        ''
      ],

      mrp: [
        0,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      sellingPrice: [
        0,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      gst: [
        18,
        [
          Validators.required,
          Validators.min(0),
          Validators.max(100)
        ]
      ],

      stock: [
        0,
        [
          Validators.required,
          Validators.min(0)
        ]
      ],

      weight: [
        null,
        Validators.min(0)
      ],

      unit: [
        '',
        Validators.maxLength(20)
      ],

      activeIngredient: [
        '',
        Validators.maxLength(500)
      ],

      dosage: [
        '',
        Validators.maxLength(1000)
      ],

      applicationMethod: [
        '',
        Validators.maxLength(1000)
      ],

      benefits: [
        '',
        Validators.maxLength(2000)
      ],

      usageInstructions: [
        '',
        Validators.maxLength(3000)
      ],

      safetyPrecautions: [
        '',
        Validators.maxLength(3000)
      ],

      manufacturer: [
        '',
        Validators.maxLength(200)
      ],

      countryOfOrigin: [
        '',
        Validators.maxLength(100)
      ],

      expiryDate: [
        null
      ],

      isFeatured: [
        false
      ]

    });
  }

  private loadDropdownData(): void {

    this.isLoading = true;
    this.errorMessage = '';

    this.changeDetector.detectChanges();

    this.categoryService
      .getCategories()
      .subscribe({

        next: (categoryResponse) => {

          if (categoryResponse.success) {

            this.categories =
              categoryResponse.data
                .filter(category =>
                  category.isActive
                )
                .sort(
                  (a, b) =>
                    a.displayOrder -
                    b.displayOrder
                );

          } else {

            this.errorMessage =
              categoryResponse.message ||
              'Unable to load categories.';

            this.isLoading = false;

            this.changeDetector.detectChanges();

            return;
          }

          this.loadBrands();
        },

        error: (error) => {

          console.error(
            'Failed to load categories:',
            error
          );

          this.errorMessage =
            'Unable to load categories. Please try again.';

          this.isLoading = false;

          this.changeDetector.detectChanges();
        }

      });
  }

  private loadBrands(): void {

    this.brandService
      .getBrands()
      .subscribe({

        next: (brandResponse) => {

          if (brandResponse.success) {

            this.brands =
              brandResponse.data
                .filter(brand =>
                  brand.isActive
                );

          } else {

            this.errorMessage =
              brandResponse.message ||
              'Unable to load brands.';
          }

          this.isLoading = false;

          this.changeDetector.detectChanges();
        },

        error: (error) => {

          console.error(
            'Failed to load brands:',
            error
          );

          this.errorMessage =
            'Unable to load brands. Please try again.';

          this.isLoading = false;

          this.changeDetector.detectChanges();
        }

      });
  }

  isInvalid(
    controlName: string
  ): boolean {

    const control =
      this.productForm.get(controlName);

    return !!(
      control &&
      control.invalid &&
      (
        control.dirty ||
        control.touched
      )
    );
  }

  getErrorMessage(
    controlName: string
  ): string {

    const control =
      this.productForm.get(controlName);

    if (!control?.errors) {
      return '';
    }

    if (control.errors['required']) {
      return 'This field is required.';
    }

    if (control.errors['minlength']) {
      return `Minimum ${control.errors['minlength'].requiredLength} characters required.`;
    }

    if (control.errors['maxlength']) {
      return `Maximum ${control.errors['maxlength'].requiredLength} characters allowed.`;
    }

    if (control.errors['min']) {
      return `Value must be at least ${control.errors['min'].min}.`;
    }

    if (control.errors['max']) {
      return `Value cannot exceed ${control.errors['max'].max}.`;
    }

    return 'Invalid value.';
  }

  submitProduct(): void {

    if (this.isSubmitting) {
      return;
    }

    if (this.productForm.invalid) {

      this.productForm.markAllAsTouched();

      return;
    }

    this.isSubmitting = true;
    this.errorMessage = '';

    const formValue =
      this.productForm.getRawValue();

    const request: CreateProductRequest = {

      categoryId:
        Number(formValue.categoryId),

      brandId:
        Number(formValue.brandId),

      name:
        formValue.name.trim(),

      shortDescription:
        this.toNullableString(
          formValue.shortDescription
        ),

      description:
        this.toNullableString(
          formValue.description
        ),

      mrp:
        Number(formValue.mrp),

      sellingPrice:
        Number(formValue.sellingPrice),

      gst:
        Number(formValue.gst),

      stock:
        Number(formValue.stock),

      weight:
        formValue.weight === null ||
        formValue.weight === ''
          ? null
          : Number(formValue.weight),

      unit:
        this.toNullableString(
          formValue.unit
        ),

      activeIngredient:
        this.toNullableString(
          formValue.activeIngredient
        ),

      dosage:
        this.toNullableString(
          formValue.dosage
        ),

      applicationMethod:
        this.toNullableString(
          formValue.applicationMethod
        ),

      benefits:
        this.toNullableString(
          formValue.benefits
        ),

      usageInstructions:
        this.toNullableString(
          formValue.usageInstructions
        ),

      safetyPrecautions:
        this.toNullableString(
          formValue.safetyPrecautions
        ),

      manufacturer:
        this.toNullableString(
          formValue.manufacturer
        ),

      countryOfOrigin:
        this.toNullableString(
          formValue.countryOfOrigin
        ),

      expiryDate:
        formValue.expiryDate || null,

      isFeatured:
        Boolean(formValue.isFeatured)

    };

    this.productService
      .createProduct(request)
      .subscribe({

        next: () => {

          this.isSubmitting = false;

          this.router.navigate([
            '/admin/products'
          ]);

        },

        error: (error) => {

          console.error(
            'Failed to create product:',
            error
          );

          this.isSubmitting = false;

          this.errorMessage =
            error?.error?.message ||
            'Unable to create the product. Please try again.';

          this.changeDetector.detectChanges();
        }

      });
  }

  private toNullableString(
    value: unknown
  ): string | null {

    if (
      value === null ||
      value === undefined
    ) {
      return null;
    }

    const text =
      String(value).trim();

    return text.length > 0
      ? text
      : null;
  }

  cancel(): void {

    this.router.navigate([
      '/admin/products'
    ]);
  }
}