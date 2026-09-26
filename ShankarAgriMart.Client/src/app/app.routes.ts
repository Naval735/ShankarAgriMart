import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';

import { OrdersListComponent } from './features/orders/pages/orders-list/orders-list';

import { AdminOrdersComponent } from './features/admin/pages/orders/admin-orders/admin-orders';

import { AdminOrderDetailsComponent } from './features/admin/pages/orders/order-details/admin-order-details';

import { adminGuard } from './core/guards/admin.guard';

import { AdminDashboardComponent } from './features/admin/pages/dashboard/admin-dashboard';

import { RegisterComponent } from './features/auth/pages/register/register';

import { AboutComponent } from './features/about/pages/about/about';

import { ServicesComponent } from './features/services/pages/services/services';

import { AdminProductsComponent } from './features/admin/pages/products/admin-products/admin-products';

export const routes: Routes = [

  // ==========================================
  // AUTHENTICATION
  // ==========================================

  {
    path: 'login',
    loadComponent: () =>
      import('./features/auth/pages/login/login')
        .then(m => m.LoginComponent)
  },

  {
    path: 'register',
    component: RegisterComponent
  },


  // ==========================================
  // PUBLIC PAGES
  // ==========================================

  {
    path: 'home',
    loadComponent: () =>
      import('./features/home/pages/home/home/home')
        .then(m => m.HomeComponent)
  },

  {
    path: 'products',
    loadComponent: () =>
      import('./features/products/pages/products/products')
        .then(m => m.ProductsComponent)
  },

  {
    path: 'products/:id',
    loadComponent: () =>
      import('./features/products/pages/product-details/product-details')
        .then(m => m.ProductDetailsComponent)
  },

  {
    path: 'about',
    component: AboutComponent
  },

  {
    path: 'services',
    component: ServicesComponent
  },


  // ==========================================
  // CUSTOMER PAGES
  // ==========================================

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
    path: 'orders',
    component: OrdersListComponent,
    canActivate: [authGuard]
  },

  {
    path: 'orders/:id',
    canActivate: [authGuard],
    loadComponent: () =>
      import('./features/orders/pages/order-details/order-details')
        .then(m => m.OrderDetailsComponent)
  },


  // ==========================================
  // ADMIN PAGES
  // ==========================================

  {
    path: 'admin/dashboard',
    component: AdminDashboardComponent,
    canActivate: [adminGuard]
  },

  {
    path: 'admin/orders',
    component: AdminOrdersComponent,
    canActivate: [adminGuard]
  },

  {
    path: 'admin/orders/:id',
    component: AdminOrderDetailsComponent,
    canActivate: [adminGuard]
  },

 {
  path: 'admin/products/add',
  loadComponent: () =>
    import('./features/admin/pages/products/admin-products/add-product/add-product')
      .then(m => m.AddProductComponent),
  canActivate: [adminGuard]
},

  {
    path: 'admin/products',
    component: AdminProductsComponent,
    canActivate: [adminGuard]
  },


  // ==========================================
  // DEFAULT ROUTE
  // ==========================================

  {
    path: '',
    redirectTo: 'home',
    pathMatch: 'full'
  },


  // ==========================================
  // INVALID ROUTES
  // ==========================================

  {
    path: '**',
    redirectTo: 'home'
  }

];