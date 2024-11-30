export interface CreateProductRequest {
  name: string;
  brand: string;
  price: number;
  stock: number;
  categories: number[];
  description: string;
}
