export interface Category {
  id: number;
  name: string;
  description: string;
  imageUrl: string | null;
  displayOrder: number;
  isActive: boolean;
}