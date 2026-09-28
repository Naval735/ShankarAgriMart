
import {
  Component,
  OnInit,
  inject,
  ChangeDetectorRef
} from '@angular/core';

import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { CategoryService } from '../../../products/services/category.service';
import { Category } from '../../../products/models/category.model';

@Component({
  selector: 'app-categories',
  standalone: true,
  imports: [
    CommonModule,
    RouterLink
  ],
  templateUrl: './categories.html',
  styleUrl: './categories.css'
})
export class CategoriesComponent implements OnInit {

  private readonly categoryService = inject(CategoryService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  categories: Category[] = [];

  searchTerm = '';
  selectedStatus = 'all';

  isLoading = true;
  errorMessage = '';

  // Delete state and feedback
  deletingCategoryId: number | null = null;
  successMessage = '';
  deleteErrorMessage = '';

  ngOnInit(): void {
    this.loadCategories();
  }

  // Load all categories
  loadCategories(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.categoryService.getCategories().subscribe({
      next: (response) => {
        if (response.success) {
          this.categories = (response.data ?? [])
            .sort((a, b) => a.displayOrder - b.displayOrder);
        } else {
          this.errorMessage =
            response.message || 'Unable to load categories.';
        }

        this.isLoading = false;
        this.changeDetector.detectChanges();
      },

      error: (error) => {
        console.error('Failed to load categories:', error);

        this.errorMessage =
          error?.error?.message ||
          'Unable to load categories. Please try again.';

        this.isLoading = false;
        this.changeDetector.detectChanges();
      }
    });
  }

  // Delete category
  deleteCategory(category: Category): void {
    if (this.deletingCategoryId !== null) {
      return;
    }

    const confirmed = window.confirm(
      `Are you sure you want to delete "${category.name}"?`
    );

    if (!confirmed) {
      return;
    }

    this.deletingCategoryId = category.id;
    this.successMessage = '';
    this.deleteErrorMessage = '';

    this.categoryService.deleteCategory(category.id).subscribe({
      next: (response) => {
        if (response.success) {
          this.categories = this.categories.filter(
            item => item.id !== category.id
          );

          this.successMessage =
            response.message || 'Category deleted successfully.';
        } else {
          this.deleteErrorMessage =
            response.message || 'Unable to delete category.';
        }

        this.deletingCategoryId = null;
        this.changeDetector.detectChanges();
      },

      error: (error) => {
        console.error('Failed to delete category:', error);

        this.deleteErrorMessage =
          error?.error?.message ||
          'Unable to delete category. Please try again.';

        this.deletingCategoryId = null;
        this.changeDetector.detectChanges();
      }
    });
  }

  // Search and status filtering
  get filteredCategories(): Category[] {
    const search = this.searchTerm.trim().toLowerCase();

    return this.categories.filter((category) => {
      const matchesSearch =
        !search ||
        category.name.toLowerCase().includes(search) ||
        (category.description ?? '').toLowerCase().includes(search);

      const matchesStatus =
        this.selectedStatus === 'all' ||
        (this.selectedStatus === 'active' && category.isActive) ||
        (this.selectedStatus === 'inactive' && !category.isActive);

      return matchesSearch && matchesStatus;
    });
  }

  // Summary counts
  get totalCategories(): number {
    return this.categories.length;
  }

  get activeCategories(): number {
    return this.categories.filter(category => category.isActive).length;
  }

  get inactiveCategories(): number {
    return this.categories.filter(category => !category.isActive).length;
  }

  // Search and filter handlers
  onSearchChange(value: string): void {
    this.searchTerm = value;
  }

  onStatusChange(value: string): void {
    this.selectedStatus = value;
  }
}