export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  firstName: string;
  lastName?: string;
  email: string;
  phone: string;
  password: string;
}

export interface AuthResponse {
  userId: number;
  firstName: string;
  email: string;
  role: string;
  token: string;
  expiresAt: string;
}