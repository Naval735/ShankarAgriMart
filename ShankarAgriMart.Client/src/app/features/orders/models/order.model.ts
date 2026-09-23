export interface CreateOrderRequest {
  addressId: number;
  paymentMethod: 'ONLINE' | 'COD';
}

export interface OrderItem {
  id: number;
  productId: number;
  productName: string;
  quantity: number;
  unitPrice: number;
  gst: number;
  total: number;
}

export interface Order {
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
  paymentMethod: string | null;
  orderDate: string;
  items: OrderItem[];
}