
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

import {
  AuthService,
  AuthUser
} from '../../../features/auth/services/auth.service';

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

  isAuthChecked = true;
  isMenuOpen = false;
  isLoggingOut = false;
  isMobileSearchOpen = false;
  isProfileMenuOpen = false;

  ngOnInit(): void {
    this.authService.currentUser$.subscribe((user) => {
      this.updateAuthState(user);
    });

    this.checkAuthentication();
  }

  private updateAuthState(user: AuthUser | null): void {
    this.isAuthenticated = !!user;
    this.isAdmin = user?.role === 'Admin';

    if (!this.isAuthenticated) {
      this.closeProfileMenu();
    }
  }

  private checkAuthentication(): void {
    this.authService.getProfile<UserProfileResponse>().subscribe({
      next: (response) => {
        if (response?.success && response?.data) {
          this.authService.setCurrentUser(response.data);
        } else {
          this.authService.setCurrentUser(null);
        }

        this.isAuthChecked = true;
      },
      error: (error) => {
        console.error('Navbar authentication check failed:', error);
        this.authService.setCurrentUser(null);
        this.isAuthChecked = true;
      }
    });
  }

  toggleMenu(): void {
    this.isMenuOpen = !this.isMenuOpen;

    if (this.isMenuOpen) {
      this.closeMobileSearch();
      this.closeProfileMenu();
    }
  }

  closeMenu(): void {
    this.isMenuOpen = false;
  }

  toggleMobileSearch(): void {
    this.isMobileSearchOpen = !this.isMobileSearchOpen;

    if (this.isMobileSearchOpen) {
      this.closeMenu();
      this.closeProfileMenu();
    }
  }

  closeMobileSearch(): void {
    this.isMobileSearchOpen = false;
  }

  toggleProfileMenu(): void {
    this.isProfileMenuOpen = !this.isProfileMenuOpen;
  }

  closeProfileMenu(): void {
    this.isProfileMenuOpen = false;
  }

  logout(): void {
    if (this.isLoggingOut) {
      return;
    }

    this.isLoggingOut = true;

    this.authService.logout().subscribe({
      next: () => {
        this.authService.setCurrentUser(null);
        this.isLoggingOut = false;

        this.closeMenu();
        this.closeMobileSearch();
        this.closeProfileMenu();

        this.router.navigate(['/home']);
      },

      error: (error) => {
        console.error('Logout failed:', error);

        this.authService.setCurrentUser(null);
        this.isLoggingOut = false;

        this.closeMenu();
        this.closeMobileSearch();
        this.closeProfileMenu();

        this.router.navigate(['/home']);
      }
    });
  }
}