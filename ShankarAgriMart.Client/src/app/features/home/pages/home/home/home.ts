import {
  ChangeDetectorRef,
  Component,
  inject,
  OnInit
} from '@angular/core';

import { RouterLink } from '@angular/router';

import { NavbarComponent } from '../../../../../layout/components/navbar/navbar';

import { CategoryService } from '../../../../products/services/category.service';
import { Category } from '../../../../products/models/category.model';

@Component({
  selector: 'app-home',
  standalone: true,
  templateUrl: './home.html',
  styleUrl: './home.css',
  imports: [NavbarComponent, RouterLink]
})
export class HomeComponent {

  private readonly categoryService = inject(CategoryService);
  private readonly changeDetector = inject(ChangeDetectorRef);

  categories: Category[] = [];

  isCategoriesLoading = true;
  categoriesError = '';

  ngOnInit(): void {
    this.loadCategories();
  }

  private loadCategories(): void {

    this.categoryService.getCategories().subscribe({

      next: (response) => {

        if (response.success) {

          this.categories = response.data
            .filter(category => category.isActive)
            .sort(
              (a, b) =>
                a.displayOrder - b.displayOrder
            );

        } else {

          this.categoriesError = response.message;

        }

        this.isCategoriesLoading = false;

        // Update the UI after the API response
        this.changeDetector.detectChanges();
      },

      error: (error) => {

        console.error(
          'Category API error:',
          error
        );

        this.categoriesError =
          'Unable to load categories. Please try again later.';

        this.isCategoriesLoading = false;

        // Update the UI when the request fails
        this.changeDetector.detectChanges();
      }

    });
  }
}