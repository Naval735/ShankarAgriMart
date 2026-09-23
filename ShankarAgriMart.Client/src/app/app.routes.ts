import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
export const routes: Routes = [

  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login')
        .then(m => m.LoginComponent)
  },

  {
    path: 'home',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/home/pages/home/home/home')
        .then(m => m.HomeComponent)
  },

  {
    path: 'products',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/products/pages/products/products')
        .then(m => m.ProductsComponent)
  },

  {
  path: 'products/:id',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./features/products/pages/product-details/product-details')
      .then(m => m.ProductDetailsComponent)
},
{
  path: 'cart',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./features/cart/pages/cart/cart')
      .then(m => m.CartComponent)
},
{
  path: 'checkout',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./features/checkout/pages/checkout/checkout')
      .then(m => m.CheckoutComponent)
},
{
  path: 'orders/:id',
  canActivate: [authGuard],
  loadComponent: () =>
    import('./features/orders/pages/order-details/order-details')
      .then(m => m.OrderDetailsComponent)
},
  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },

  {
    path: '**',
    redirectTo: 'home'
  }

];