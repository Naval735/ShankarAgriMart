export interface Product {
  id: number;
  categoryId: number;
  categoryName: string;

  brandId: number;
  brandName: string;

  name: string;
  slug: string;
  sku: string;

  shortDescription: string;
  description: string;

  mrp: number;
  sellingPrice: number;
  gst: number;
  stock: number;

  weight: number;
  unit: string;

  activeIngredient: string;
  dosage: string;
  applicationMethod: string;
  benefits: string;
  usageInstructions: string;
  safetyPrecautions: string;

  manufacturer: string;
  countryOfOrigin: string;

  expiryDate: string;

  isFeatured: boolean;
  isActive: boolean;

  images: string[];
}