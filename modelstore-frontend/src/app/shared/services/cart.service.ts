import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { CartItem } from '../interfaces/cart-item';
import { Product } from '../interfaces/product';
import { Guid } from 'guid-typescript';

@Injectable({
  providedIn: 'root',
})
export class CartService {
  private cart: CartItem[] = [];
  private cartSubject = new BehaviorSubject<CartItem[]>([]);

  constructor() {}

  getCart() {
    return this.cartSubject.asObservable();
  }

  addToCart(product: Product): void {
    const existingItem = this.cart.find(
      (item) => item.product.id === product.id
    );

    if (existingItem) {
      existingItem.quantity += 1;
    } else {
      this.cart.push({ product, quantity: 1 });
    }

    this.updateCart();
  }

  removeFromCart(productId: Guid): void {
    this.cart = this.cart.filter((item) => item.product.id !== productId);
    this.updateCart();
  }

  decreaseQuantity(productId: Guid): void {
    const existingItem = this.cart.find(
      (item) => item.product.id === productId
    );

    if (existingItem) {
      existingItem.quantity -= 1;

      if (existingItem.quantity <= 0) {
        this.removeFromCart(productId);
      } else {
        this.updateCart();
      }
    }
  }

  clearCart(): void {
    this.cart = [];
    this.updateCart();
  }

  private updateCart(): void {
    this.cartSubject.next(this.cart);
  }
}
