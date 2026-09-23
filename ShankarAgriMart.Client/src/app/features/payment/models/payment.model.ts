export interface CreatePaymentResponse {
  success: boolean;
  message: string;
  data: {
    orderId: number;
    orderNumber: string;
    amount: number;
    razorpayOrderId: string;
    currency: string;
    paymentId: string | null;
    paymentStatus: string;
  };
}

export interface VerifyPaymentRequest {
  razorpayPaymentId: string;
  razorpayOrderId: string;
  razorpaySignature: string;
}

export interface VerifyPaymentResponse {
  success: boolean;
  message: string;
  data?: unknown;
}