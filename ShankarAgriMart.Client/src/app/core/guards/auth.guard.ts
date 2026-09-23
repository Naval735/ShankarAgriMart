import { inject } from '@angular/core';
import {
  CanActivateFn,
  Router
} from '@angular/router';
import {
  catchError,
  map,
  of
} from 'rxjs';

import { AuthService } from '../../features/auth/services/auth.service';

export const authGuard: CanActivateFn = () => {

  const authService = inject(AuthService);
  const router = inject(Router);

  return authService.getProfile().pipe(

    map(() => {
      // Backend confirmed that the
      // current authentication cookie is valid.
      return true;
    }),

    catchError(() => {
      // User is not authenticated.
      // Return a UrlTree so Angular redirects
      // without manually navigating.
      return of(
        router.createUrlTree(['/login'])
      );
    })

  );
};