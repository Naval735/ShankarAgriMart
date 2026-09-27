export interface CreateProductRequest {
  categoryId: number;
  brandId: number;

  name: string;
  shortDescription?: string | null;
  description?: string | null;

  mrp: number;
  sellingPrice: number;
  gst: number;
  stock: number;

  weight?: number | null;
  unit?: string | null;

  activeIngredient?: string | null;
  dosage?: string | null;
  applicationMethod?: string | null;

  benefits?: string | null;
  usageInstructions?: string | null;
  safetyPrecautions?: string | null;

  manufacturer?: string | null;
  countryOfOrigin?: string | null;

  expiryDate?: string | null;

  isFeatured: boolean;
}

export interface UpdateProductRequest {
  categoryId: number;
  brandId: number;

  name: string;
  shortDescription?: string | null;
  description?: string | null;

  mrp: number;
  sellingPrice: number;
  gst: number;
  stock: number;

  weight?: number | null;
  unit?: string | null;

  activeIngredient?: string | null;
  dosage?: string | null;
  applicationMethod?: string | null;

  benefits?: string | null;
  usageInstructions?: string | null;
  safetyPrecautions?: string | null;

  manufacturer?: string | null;
  countryOfOrigin?: string | null;

  expiryDate?: string | null;

  isFeatured: boolean;
  isActive: boolean;
}