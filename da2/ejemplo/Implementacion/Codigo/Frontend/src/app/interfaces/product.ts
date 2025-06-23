export interface Product {
  id: number;
  code: string;
  name: string;
  quantity: number;
  price: number;
  hasDiscount: boolean;
  discountPercentage: number | null;
}
