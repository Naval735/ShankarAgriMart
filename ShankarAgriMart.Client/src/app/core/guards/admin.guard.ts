import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, map, of } from 'rxjs';

import { AuthService } from '../../features/auth/services/auth.service';

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

export const adminGuard: CanActivateFn = () => {

  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.getProfile<UserProfileResponse>().pipe(

    map(response => {

      if (
        response.success &&
        response.data?.role === 'Admin'
      ) {
        return true;
      }

      return router.createUrlTree(['/home']);
    }),

    catchError(() => {
      return of(
        router.createUrlTree(['/login'])
      );
    })

  );
};