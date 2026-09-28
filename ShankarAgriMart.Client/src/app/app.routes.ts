
import { Routes } from '@angular/router';

import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

import { OrdersListComponent } from './features/orders/pages/orders-list/orders-list';

import { AdminOrdersComponent } from './features/admin/pages/orders/admin-orders/admin-orders';
import { AdminOrderDetailsComponent } from './features/admin/pages/orders/order-details/admin-order-details';
import { AdminDashboardComponent } from './features/admin/pages/dashboard/admin-dashboard';
import { AdminProductsComponent } from './features/admin/pages/products/admin-products/admin-products';

import { RegisterComponent } from './features/auth/pages/register/register';
import { AboutComponent } from './features/about/pages/about/about';
import { ServicesComponent } from './features/services/pages/services/services';

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
    path: 'services',
    loadComponent: () =>
      import('./features/services/pages/services/services')
        .then(m => m.ServicesComponent)
  },

  {
    path: 'about',
    loadComponent: () =>
      import('./features/about/pages/about/about')
        .then(m => m.AboutComponent)
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

  // Admin Categories
  {
    path: 'admin/categories',
    loadComponent: () =>
      import('./features/admin/pages/categories/categories')
        .then(m => m.CategoriesComponent),
    canActivate: [adminGuard]
  },

  // Add Product
  {
    path: 'admin/products/add',
    loadComponent: () =>
      import('./features/admin/pages/products/admin-products/add-product/add-product')
        .then(m => m.AddProductComponent),
    canActivate: [adminGuard]
  },

  // Edit Product
  {
    path: 'admin/products/edit/:id',
    loadComponent: () =>
      import('./features/admin/pages/products/admin-products/edit-product/edit-product')
        .then(m => m.EditProductComponent),
    canActivate: [adminGuard]
  },

  {
    path: 'admin/products',
    component: AdminProductsComponent,
    canActivate: [adminGuard]
  },

// Add Category
{
  path: 'admin/categories/add',
  loadComponent: () =>
    import('./features/admin/pages/categories/add-category/add-category')
      .then(m => m.AddCategoryComponent),
  canActivate: [adminGuard]
},

// Edit Category
{
  path: 'admin/categories/edit/:id',
  loadComponent: () =>
    import('./features/admin/pages/categories/edit-category/edit-category')
      .then(m => m.EditCategoryComponent),
  canActivate: [adminGuard]
},
// Admin Brands
{
  path: 'admin/brands',
  loadComponent: () =>
    import('./features/admin/pages/brands/brands')
      .then(m => m.BrandsComponent),
  canActivate: [adminGuard]
},

// Add Brand
{
  path: 'admin/brands/add',
  loadComponent: () =>
    import('./features/admin/pages/brands/add-brand/add-brand')
      .then(m => m.AddBrandComponent),
  canActivate: [adminGuard]
},

// Edit Brand
{
  path: 'admin/brands/edit/:id',
  loadComponent: () =>
    import('./features/admin/pages/brands/edit-brand/edit-brand')
      .then(m => m.EditBrandComponent),
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