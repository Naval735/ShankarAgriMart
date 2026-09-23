import { Component, inject } from '@angular/core';
import { Router, RouterLink,  RouterLinkActive } from '@angular/router';

import { AuthService } from '../../../features/auth/services/auth.service';

@Component({
  selector: 'app-navbar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './navbar.html',
  styleUrl: './navbar.css'
})
export class NavbarComponent {

  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  isLoggingOut = false;

  logout(): void {
    this.isLoggingOut = true;

    this.authService.logout().subscribe({
      next: () => {
        this.isLoggingOut = false;
        this.router.navigate(['/login']);
      },

      error: (error) => {
        console.error('Logout failed:', error);

        this.isLoggingOut = false;
        this.router.navigate(['/login']);
      }
    });
  }
}