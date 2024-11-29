export interface UpdateProductRequest {
  name: string;
  brand: string;
  price: number;
  stock: number;
  categories: string[];
  description: string;
}
