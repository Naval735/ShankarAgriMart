export interface AdminOrderItem {
  id: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  gst: number;
  total: number;
}

export interface AdminOrder {
  id: number;
  orderNumber: string;
  userId: number;
  addressId: number;

  subTotal: number;
  gst: number;
  deliveryCharge: number;
  discount: number;
  grandTotal: number;

  paymentStatus: number;
  orderStatus: number;
  paymentMethod: number | string | null;

  orderDate: string;

  items: AdminOrderItem[];
}

export interface AdminOrderApiResponse<T> {
  success: boolean;
  data: T;
  message?: string;
}