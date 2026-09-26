export interface Brand {
  id: number;
  name: string;
  logoUrl?: string | null;
  description?: string | null;
  isActive: boolean;
}