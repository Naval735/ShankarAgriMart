import {
  ChangeDetectorRef,
  Component,
  inject,
  OnDestroy,
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

  imports: [
    NavbarComponent,
    RouterLink
  ]
})
export class HomeComponent
  implements OnInit, OnDestroy {

  private readonly categoryService =
    inject(CategoryService);

  private readonly changeDetector =
    inject(ChangeDetectorRef);


  // =========================================================
  // CATEGORIES
  // =========================================================

  categories: Category[] = [];

  isCategoriesLoading = true;

  categoriesError = '';


  // =========================================================
  // HERO CAROUSEL
  // =========================================================

  currentSlide = 0;

  private carouselTimer?: ReturnType<typeof setInterval>;


  slides = [

    {
      eyebrow: 'INDIA’S ONLINE AGRISTORE',

      title: 'Everything Farmers Need,',

      highlight: 'In One Place.',

      description:
        'Shop quality agricultural products, pesticides, fertilizers and farming essentials with ease.',

      buttonText: 'Shop Products',

      buttonLink: '/products',

      image:
        'https://images.unsplash.com/photo-1625246333195-78d9c38ad449?auto=format&fit=crop&w=1400&q=85'
    },


    {
      eyebrow: 'QUALITY AGRICULTURAL PRODUCTS',

      title: 'Grow Better With',

      highlight: 'Trusted Products.',

      description:
        'Discover reliable seeds, crop protection products, fertilizers and farming solutions.',

      buttonText: 'Explore Products',

      buttonLink: '/products',

      image:
        'https://images.unsplash.com/photo-1492496913980-501348b61469?auto=format&fit=crop&w=1400&q=85'
    },


    {
      eyebrow: 'SMART FARMING SOLUTIONS',

      title: 'From Farm',

      highlight: 'To Better Harvests.',

      description:
        'Everything you need to support healthier crops and more productive farming.',

      buttonText: 'Start Shopping',

      buttonLink: '/products',

      image:
        'https://images.unsplash.com/photo-1501004318641-b39e6451bec6?auto=format&fit=crop&w=1400&q=85'
    }

  ];


  // =========================================================
  // INITIALIZATION
  // =========================================================

  ngOnInit(): void {

    this.loadCategories();

    this.startCarousel();

  }


  // =========================================================
  // CLEANUP
  // =========================================================

  ngOnDestroy(): void {

    this.stopCarousel();

  }


  // =========================================================
  // CATEGORY API
  // =========================================================

  private loadCategories(): void {

    this.categoryService
      .getCategories()
      .subscribe({

        next: (response) => {

          if (response.success) {

            this.categories =
              response.data
                .filter(
                  category =>
                    category.isActive
                )
                .sort(
                  (a, b) =>
                    a.displayOrder -
                    b.displayOrder
                );

          } else {

            this.categoriesError =
              response.message;

          }

          this.isCategoriesLoading =
            false;

          this.changeDetector.detectChanges();

        },


        error: (error) => {

          console.error(
            'Category API error:',
            error
          );

          this.categoriesError =
            'Unable to load categories. Please try again later.';

          this.isCategoriesLoading =
            false;

          this.changeDetector.detectChanges();

        }

      });

  }


  // =========================================================
  // CAROUSEL
  // =========================================================

  startCarousel(): void {

    this.stopCarousel();

    this.carouselTimer =
      setInterval(() => {

        this.nextSlide();

      }, 4000);

  }


  stopCarousel(): void {

    if (this.carouselTimer) {

      clearInterval(
        this.carouselTimer
      );

      this.carouselTimer = undefined;

    }

  }


  nextSlide(): void {

    this.currentSlide =
      (this.currentSlide + 1)
      % this.slides.length;

    this.changeDetector.detectChanges();

  }


  previousSlide(): void {

    this.currentSlide =
      this.currentSlide === 0
        ? this.slides.length - 1
        : this.currentSlide - 1;

    this.changeDetector.detectChanges();

  }


  goToSlide(index: number): void {

    this.currentSlide = index;

    // Restart the automatic timer
    // after manual navigation.
    this.startCarousel();

    this.changeDetector.detectChanges();

  }

}