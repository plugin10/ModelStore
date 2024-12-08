import { Component, OnInit, OnDestroy } from '@angular/core';
import { CartItem } from '../../shared/interfaces/cart-item';
import { MessageService } from 'primeng/api';
import { CartService } from '../../shared/services/cart.service';
import { Subscription } from 'rxjs';
import { Guid } from 'guid-typescript';
import { ButtonModule } from 'primeng/button';
import { RippleModule } from 'primeng/ripple';
import { ToastModule } from 'primeng/toast';
import { TableModule } from 'primeng/table';
import { Product } from '../../shared/interfaces/product';
import { ImageModule } from 'primeng/image';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-cart',
  standalone: true,
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
  imports: [
    ButtonModule,
    RippleModule,
    ToastModule,
    TableModule,
    ImageModule,
    CommonModule,
  ],
  providers: [MessageService],
})
export class CartComponent implements OnInit, OnDestroy {
  cartItems: CartItem[] = [];
  totalSum: number = 0;
  private subscriptions: Subscription = new Subscription();

  constructor(
    private cartService: CartService,
    private messageService: MessageService
  ) {}

  ngOnInit(): void {
    this.subscriptions.add(
      this.cartService.getCart().subscribe((cart) => {
        this.cartItems = cart;
        this.totalSum = this.cartService.getTotalSum();
      })
    );
  }

  increaseQuantity(product: Product): void {
    console.log(product);
    this.cartService.addToCart(product);
  }

  decreaseQuantity(productId: Guid): void {
    console.log(productId);
    this.cartService.decreaseQuantity(productId);
  }

  removeFromCart(productId: Guid): void {
    this.cartService.removeFromCart(productId);
  }

  clearCart(): void {
    this.cartService.clearCart();
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
