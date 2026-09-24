import {
  Component,
  inject,
  OnInit
} from '@angular/core';

import {
  FormsModule
} from '@angular/forms';

import {
  Router,
  RouterLink
} from '@angular/router';

import { AuthService } from '../../../features/auth/services/auth.service';


interface UserProfileResponse {
  success: boolean;

  data: {
    id: number;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    emailVerified: boolean;
    role: string;
  };
}


@Component({
  selector: 'app-navbar',

  imports: [
    RouterLink,
    FormsModule
  ],

  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent implements OnInit {

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);


  isAuthenticated = false;
  isAdmin = false;

  isAuthChecked = false;

  isLoggingOut = false;

  isMenuOpen = false;

  searchTerm = '';


  ngOnInit(): void {

    this.checkAuthentication();

  }


  private checkAuthentication(): void {

    this.authService
      .getProfile<UserProfileResponse>()
      .subscribe({

        next: (response) => {

          if (
            response.success &&
            response.data
          ) {

            this.isAuthenticated = true;

            this.isAdmin =
              response.data.role === 'Admin';

          }
          else {

            this.isAuthenticated = false;
            this.isAdmin = false;

          }

          this.isAuthChecked = true;

        },

        error: () => {

          this.isAuthenticated = false;
          this.isAdmin = false;

          this.isAuthChecked = true;

        }

      });

  }


  toggleMenu(): void {

    this.isMenuOpen =
      !this.isMenuOpen;

  }


  closeMenu(): void {

    this.isMenuOpen = false;

  }


  searchProducts(event: Event): void {

    event.preventDefault();

    const search =
      this.searchTerm.trim();

    this.closeMenu();

    if (!search) {

      this.router.navigate(['/products']);

      return;

    }

    this.router.navigate(
      ['/products'],
      {
        queryParams: {
          search
        }
      }
    );

  }


  logout(): void {

    this.isLoggingOut = true;

    this.authService
      .logout()
      .subscribe({

        next: () => {

          this.isLoggingOut = false;

          this.isAuthenticated = false;
          this.isAdmin = false;

          this.closeMenu();

          this.router.navigate(['/login']);

        },

        error: (error) => {

          console.error(
            'Logout failed:',
            error
          );

          this.isLoggingOut = false;

          this.isAuthenticated = false;
          this.isAdmin = false;

          this.closeMenu();

          this.router.navigate(['/login']);

        }

      });

  }

}