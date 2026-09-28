import {
  ChangeDetectorRef,
  Component,
  OnInit,
  inject
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { BrandService } from '../../../products/services/brand.service';
import { Brand } from '../../../products/models/brand.model';

@Component({
  selector: 'app-brands',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './brands.html',
  styleUrl: './brands.css'
})
export class BrandsComponent implements OnInit {
  private readonly brandService = inject(BrandService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  brands: Brand[] = [];

  searchTerm = '';
  selectedStatus = 'all';

  isLoading = true;
  errorMessage = '';

  deletingBrandId: number | null = null;
  successMessage = '';
  deleteErrorMessage = '';

  ngOnInit(): void {
    this.loadBrands();
  }

  loadBrands(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.brandService.getBrands().subscribe({
      next: (response) => {
        if (response.success) {
          this.brands = [...(response.data ?? [])]
            .sort((a, b) => a.name.localeCompare(b.name));
        } else {
          this.errorMessage =
            response.message || 'Unable to load brands.';
        }

        this.isLoading = false;
        this.changeDetector.detectChanges();
      },
      error: (error) => {
        console.error('Failed to load brands:', error);

        this.errorMessage =
          error?.error?.message ||
          'Unable to load brands. Please try again.';

        this.isLoading = false;
        this.changeDetector.detectChanges();
      }
    });
  }

  deleteBrand(brand: Brand): void {
    if (this.deletingBrandId !== null) {
      return;
    }

    const confirmed = window.confirm(
      `Are you sure you want to delete "${brand.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.deletingBrandId = brand.id;
    this.successMessage = '';
    this.deleteErrorMessage = '';

    this.brandService.deleteBrand(brand.id).subscribe({
      next: (response) => {
        if (response.success) {
          this.brands = this.brands.filter(
            item => item.id !== brand.id
          );

          this.successMessage =
            response.message || 'Brand deleted successfully.';
        } else {
          this.deleteErrorMessage =
            response.message || 'Unable to delete brand.';
        }

        this.deletingBrandId = null;
        this.changeDetector.detectChanges();
      },
      error: (error) => {
        console.error('Failed to delete brand:', error);

        this.deleteErrorMessage =
          error?.error?.message ||
          'Unable to delete brand. Please try again.';

        this.deletingBrandId = null;
        this.changeDetector.detectChanges();
      }
    });
  }

  get filteredBrands(): Brand[] {
    const search = this.searchTerm.trim().toLowerCase();

    return this.brands.filter((brand) => {
      const matchesSearch =
        !search ||
        brand.name.toLowerCase().includes(search) ||
        (brand.description ?? '').toLowerCase().includes(search);

      const matchesStatus =
        this.selectedStatus === 'all' ||
        (this.selectedStatus === 'active' && brand.isActive) ||
        (this.selectedStatus === 'inactive' && !brand.isActive);

      return matchesSearch && matchesStatus;
    });
  }

  get totalBrands(): number {
    return this.brands.length;
  }

  get activeBrands(): number {
    return this.brands.filter(brand => brand.isActive).length;
  }

  get inactiveBrands(): number {
    return this.brands.filter(brand => !brand.isActive).length;
  }

  onSearchChange(value: string): void {
    this.searchTerm = value;
  }

  onStatusChange(value: string): void {
    this.selectedStatus = value;
  }
}