export interface AdminDashboard {
  totalProducts: number;
  totalCustomers: number;
  totalOrders: number;
  totalRevenue: number;

  pendingOrders: number;
  confirmedOrders: number;
  shippedOrders: number;
  deliveredOrders: number;
  cancelledOrders: number;
}

export interface AdminDashboardApiResponse {
  success: boolean;
  data: AdminDashboard;
  message?: string;
}