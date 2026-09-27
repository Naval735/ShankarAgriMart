import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
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
  ActivatedRoute,
  Router,
  RouterLink
} from '@angular/router';

import {
  AdminProductService
} from '../../../../services/admin-product.service';

import {
  CategoryService
} from '../../../../../products/services/category.service';

import {
  BrandService
} from '../../../../../products/services/brand.service';

import {
  Product
} from '../../../../../products/models/product.model';

import {
  Category
} from '../../../../../products/models/category.model';

import {
  Brand
} from '../../../../../products/models/brand.model';

import {
  UpdateProductRequest
} from '../../../../../products/models/product-request.model';

@Component({
  selector: 'app-edit-product',
  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,
    RouterLink
  ],

  templateUrl: './edit-product.html',
  styleUrl: './edit-product.css'
})
export class EditProductComponent implements OnInit {

  private readonly fb =
    inject(FormBuilder);

  private readonly route =
    inject(ActivatedRoute);

  private readonly router =
    inject(Router);

  private readonly productService =
    inject(AdminProductService);

  private readonly categoryService =
    inject(CategoryService);

  private readonly brandService =
    inject(BrandService);

  private readonly changeDetector =
    inject(ChangeDetectorRef);

  productForm!: FormGroup;

  productId!: number;

  product: Product | null = null;

  categories: Category[] = [];

  brands: Brand[] = [];

  isLoading = true;

  isSubmitting = false;

  errorMessage = '';

  ngOnInit(): void {

    this.createForm();

    this.getProductId();
  }

  private createForm(): void {

    this.productForm =
      this.fb.group({

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
        ],

        isActive: [
          true
        ]

      });
  }

  private getProductId(): void {

    const id =
      Number(
        this.route.snapshot.paramMap.get('id')
      );

    if (!id || id <= 0) {

      this.errorMessage =
        'Invalid product ID.';

      this.isLoading = false;

      this.changeDetector.detectChanges();

      return;
    }

    this.productId = id;

    this.loadData();
  }

  private loadData(): void {

    this.isLoading = true;

    this.errorMessage = '';

    this.changeDetector.detectChanges();

    this.productService
      .getProductById(this.productId)
      .subscribe({

        next: (product) => {

          this.product = product;

          this.loadDropdownData();
        },

        error: (error) => {

          console.error(
            'Failed to load product:',
            error
          );

          this.errorMessage =
            error?.error?.message ||
            'Unable to load product. Please try again.';

          this.isLoading = false;

          this.changeDetector.detectChanges();
        }

      });
  }

  private loadDropdownData(): void {

    this.categoryService
      .getCategories()
      .subscribe({

        next: (categoryResponse) => {

          if (categoryResponse.success) {

            this.categories =
              categoryResponse.data
                .filter(category =>
                  category.isActive ||
                  category.id === this.product?.categoryId
                )
                .sort(
                  (a, b) =>
                    a.displayOrder -
                    b.displayOrder
                );

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
                  brand.isActive ||
                  brand.id === this.product?.brandId
                );
          }

          this.populateForm();

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

  private populateForm(): void {

    if (!this.product) {
      return;
    }

    this.productForm.patchValue({

      categoryId:
        this.product.categoryId,

      brandId:
        this.product.brandId,

      name:
        this.product.name,

      shortDescription:
        this.product.shortDescription ?? '',

      description:
        this.product.description ?? '',

      mrp:
        this.product.mrp,

      sellingPrice:
        this.product.sellingPrice,

      gst:
        this.product.gst,

      stock:
        this.product.stock,

      weight:
        this.product.weight ?? null,

      unit:
        this.product.unit ?? '',

      activeIngredient:
        this.product.activeIngredient ?? '',

      dosage:
        this.product.dosage ?? '',

      applicationMethod:
        this.product.applicationMethod ?? '',

      benefits:
        this.product.benefits ?? '',

      usageInstructions:
        this.product.usageInstructions ?? '',

      safetyPrecautions:
        this.product.safetyPrecautions ?? '',

      manufacturer:
        this.product.manufacturer ?? '',

      countryOfOrigin:
        this.product.countryOfOrigin ?? '',

      expiryDate:
        this.formatDateForInput(
          this.product.expiryDate
        ),

      isFeatured:
        this.product.isFeatured,

      isActive:
        this.product.isActive

    });
  }

  private formatDateForInput(
    value: string | null | undefined
  ): string | null {

    if (!value) {
      return null;
    }

    return value.split('T')[0];
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

    const request:
      UpdateProductRequest = {

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
        Boolean(formValue.isFeatured),

      isActive:
        Boolean(formValue.isActive)

    };

    this.productService
      .updateProduct(
        this.productId,
        request
      )
      .subscribe({

        next: () => {

          this.isSubmitting = false;

          this.router.navigate([
            '/admin/products'
          ]);

        },

        error: (error) => {

          console.error(
            'Failed to update product:',
            error
          );

          this.isSubmitting = false;

          this.errorMessage =
            error?.error?.message ||
            'Unable to update the product. Please try again.';

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