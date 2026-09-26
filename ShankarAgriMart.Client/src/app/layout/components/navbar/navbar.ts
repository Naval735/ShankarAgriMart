import {
  Component,
  OnInit,
  inject
} from '@angular/core';

import {
  Router,
  RouterLink,
  RouterLinkActive
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
  standalone: true,
  imports: [
    RouterLink,
    RouterLinkActive
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
  isMenuOpen = false;
  isLoggingOut = false;

  ngOnInit(): void {
    this.checkAuthentication();
  }

  private checkAuthentication(): void {

    this.authService
      .getProfile<UserProfileResponse>()
      .subscribe({

        next: (response) => {

          if (
            response?.success &&
            response?.data
          ) {

            this.isAuthenticated = true;

            this.isAdmin =
              response.data.role === 'Admin';

          } else {

            this.isAuthenticated = false;
            this.isAdmin = false;

          }

          this.isAuthChecked = true;
        },

        error: (error) => {

          console.error(
            'Navbar authentication check failed:',
            error
          );

          this.isAuthenticated = false;
          this.isAdmin = false;

          this.isAuthChecked = true;
        }

      });
  }

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;
  }

  closeMenu(): void {
    this.isMenuOpen = false;
  }

  logout(): void {

    if (this.isLoggingOut) {
      return;
    }

    this.isLoggingOut = true;

    this.authService
      .logout()
      .subscribe({

        next: () => {

          this.isLoggingOut = false;
          this.isAuthenticated = false;
          this.isAdmin = false;

          this.closeMenu();

          this.router.navigate(['/home']);
        },

        error: (error) => {

          console.error(
            'Logout failed:',
            error
          );

          /*
           * Even if the server logout fails,
           * clear the frontend authentication state.
           */
          this.isLoggingOut = false;
          this.isAuthenticated = false;
          this.isAdmin = false;

          this.closeMenu();

          this.router.navigate(['/home']);
        }

      });
  }
}