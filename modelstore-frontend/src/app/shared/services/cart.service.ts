import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Product } from '../interfaces/product';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private cartKey = 'cart';
  private cart: Product[] = [];
  private cartSubject = new BehaviorSubject<Product[]>([]);

  constructor() {
    const savedCart = localStorage.getItem(this.cartKey);
    if (savedCart) {
      this.cart = JSON.parse(savedCart);
      this.cartSubject.next(this.cart);
    }
  }

  getCart() {
    return this.cartSubject.asObservable();
  }

  addToCart(product: Product) {
    const existingProduct = this.cart.find((item) => item.id === product.id);
    if (existingProduct) {
      existingProduct.stock += 1;
    } else {
      this.cart.push({ ...product, stock: 1 });
    }
    this.updateCart();
  }

  removeFromCart(productId: Guid) {
    this.cart = this.cart.filter((item) => item.id.equals(productId));
    this.updateCart();
  }

  clearCart() {
    this.cart = [];
    this.updateCart();
  }

  getCartCount(): number {
    return this.cart.reduce((total, item) => total + item.stock, 0);
  }

  private updateCart() {
    this.cartSubject.next(this.cart);
    localStorage.setItem(this.cartKey, JSON.stringify(this.cart));
  }
}
