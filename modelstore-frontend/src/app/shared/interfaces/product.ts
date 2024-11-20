import { Guid } from 'guid-typescript';

export interface Product {
  id: Guid;
  name: string;
  description: string;
  price: number;
  categoryId: number;
  rating: number;
  stock: number;
  imageUrl: string;
  scale: string; // Skala modelu, np. "1:72"
  manufacturer: string;
}
