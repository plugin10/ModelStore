import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Product } from '../interfaces/product';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root',
})
export class RecommendationService {
  private baseUrl: string = 'https://localhost:7058/api/recomendations';

  constructor(private http: HttpClient) {}

  getTopSellingProducts(): Observable<Product[]> {
    return this.http.get<any>(`${this.baseUrl}/top-selling`).pipe(
      map((data) =>
        data.items.map((item: any) => ({
          id: Guid.parse(item.id),
          name: item.name,
          brand: item.brand,
          slug: item.slug,
          rating: item.rating,
          price: item.price,
          stock: item.stock,
          categories: item.categories,
          description: item.description,
          imageUrl: item.imageUrl || null,
        }))
      )
    );
  }

  getRecommendations(recommendationType: string): Observable<Product[]> {
    return this.http.get<any>(`${this.baseUrl}/${recommendationType}`).pipe(
      map((data) =>
        data.items.map((item: any) => ({
          id: Guid.parse(item.id),
          name: item.name,
          brand: item.brand,
          slug: item.slug,
          rating: item.rating,
          price: item.price,
          stock: item.stock,
          categories: item.categories,
          description: item.description,
          imageUrl: item.imageUrl || null,
        }))
      )
    );
  }
}
