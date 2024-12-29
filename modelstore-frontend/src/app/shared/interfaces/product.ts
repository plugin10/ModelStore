import { Guid } from 'guid-typescript';

export class Product {
  id: Guid;
  name: string;
  brand: string;
  slug: string;
  rating: number | null;
  price: number;
  stock: number;
  categories: number[];
  description: string;
  imageUrl?: string;
  promotion?: number;
  categoriesText: string;

  constructor(
    id: Guid,
    name: string,
    brand: string,
    slug: string,
    rating: number | null,
    price: number,
    stock: number,
    categories: number[],
    description: string,
    categoriesText: string,
    imageUrl?: string,
    promotion?: number
  ) {
    this.id = id;
    this.name = name;
    this.brand = brand;
    this.slug = slug;
    this.rating = rating;
    this.price = price;
    this.stock = stock;
    this.categories = categories;
    this.description = description;
    this.categoriesText = categoriesText;
    this.imageUrl = imageUrl;
    this.promotion = promotion;
  }
}
