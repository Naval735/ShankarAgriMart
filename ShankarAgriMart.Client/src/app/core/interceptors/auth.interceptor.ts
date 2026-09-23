import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const apiRequest = req.clone({
    withCredentials: true
  });

  return next(apiRequest);
};