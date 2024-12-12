import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateOrderRequest } from '../interfaces/requests/create-order-request';

@Injectable({
  providedIn: 'root',
})
export class OrderService {
  private apiUrl = 'https://localhost:7058/api/orders';

  constructor(private http: HttpClient) {}

  createOrder(order: CreateOrderRequest): Observable<any> {
    return this.http.post(this.apiUrl, order);
  }
}
