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
import { OrderFormComponent } from './order-form/order-form.component';
import { DynamicDialogRef, DialogService } from 'primeng/dynamicdialog';
import { OrderService } from '../../shared/services/order.service';
import { CreateOrderRequest } from '../../shared/interfaces/requests/create-order-request';
import { RecommendationService } from '../../shared/services/recommendation.service';
import { DataViewModule } from 'primeng/dataview';
import { TagModule } from 'primeng/tag';
import { RatingModule } from 'primeng/rating';

@Component({
  selector: 'app-cart',
  standalone: true,
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.scss',
  imports: [
    ButtonModule,
    RippleModule,
    ToastModule,
    TableModule,
    ImageModule,
    CommonModule,
    DataViewModule,
    TagModule,
  ],
  providers: [MessageService, DialogService],
})
export class CartComponent implements OnInit, OnDestroy {
  layout: any = 'grid';
  cartItems: CartItem[] = [];
  totalSum: number = 0;
  topSellingProducts: Product[] = [];
  private subscriptions: Subscription = new Subscription();
  ref: DynamicDialogRef | undefined;

  constructor(
    private cartService: CartService,
    private messageService: MessageService,
    private dialogService: DialogService,
    private orderService: OrderService,
    private recommendationService: RecommendationService
  ) {}

  ngOnInit(): void {
    this.subscriptions.add(
      this.cartService.getCart().subscribe((cart) => {
        this.cartItems = cart;
        this.totalSum = this.cartService.getTotalSum();
      })
    );

    this.fetchTopSellingProducts();

    this.topSellingProducts = [
      new Product(
        Guid.create(),
        'Airbrush Paint Set',
        'HobbyTech',
        'airbrush-paint-set',
        4.5,
        99.99,
        25,
        [1, 2],
        'Zestaw farb do aerografu, idealny dla modelarzy.',
        'https://via.placeholder.com/150'
      ),
      new Product(
        Guid.create(),
        'Plastic Model Kit',
        'ModelMaker',
        'plastic-model-kit',
        4.8,
        149.99,
        10,
        [3],
        'Zaawansowany zestaw do budowy modeli plastikowych.',
        'https://via.placeholder.com/150'
      ),
      new Product(
        Guid.create(),
        'Brush Set for Miniatures',
        'PaintPro',
        'brush-set-miniatures',
        4.7,
        29.99,
        50,
        [2],
        'Zestaw precyzyjnych pędzli do malowania figurek.',
        'https://via.placeholder.com/150'
      ),
    ];
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

  openOrderForm(): void {
    this.ref = this.dialogService.open(OrderFormComponent, {
      header: 'Wypełnij dane zamówienia',
      width: '400px',
    });

    this.ref.onClose.subscribe((formData: any) => {
      if (formData) {
        this.placeOrder(formData);
      } else {
        this.messageService.add({
          severity: 'info',
          summary: 'Anulowano',
          detail: 'Nie złożono zamówienia.',
        });
      }
    });
  }

  placeOrder(formData: any): void {
    if (!this.cartItems.length) {
      this.messageService.add({
        severity: 'warn',
        summary: 'Koszyk pusty',
        detail: 'Dodaj produkty przed złożeniem zamówienia.',
      });
      return;
    }

    const order: CreateOrderRequest = {
      clientName: formData.clientName,
      clientEmail: formData.clientEmail,
      clientPhone: formData.clientPhone,
      shippingAddress: formData.shippingAddress,
      elements: this.cartItems.map((item) => ({
        productId: item.product.id.toString(),
        quantity: item.quantity,
      })),
    };

    console.log(order);

    this.orderService.createOrder(order).subscribe({
      next: () => {
        this.messageService.add({
          severity: 'success',
          summary: 'Zamówienie złożone',
          detail: 'Twoje zamówienie zostało pomyślnie złożone.',
        });
        this.clearCart();
      },
      error: (err) => {
        console.error('Błąd podczas składania zamówienia:', err);
        this.messageService.add({
          severity: 'error',
          summary: 'Błąd',
          detail: 'Nie udało się złożyć zamówienia. Spróbuj ponownie później.',
        });
      },
    });
  }

  fetchTopSellingProducts(): void {
    // this.recommendationService.getTopSellingProducts().subscribe({
    //   // next: (products) => (this.topSellingProducts = products),
    //   next: (products) => {
    //     this.topSellingProducts = products;
    //     console.log('Bestsellery:', this.topSellingProducts);
    //   },
    //   error: (err) => console.error('Błąd pobierania bestsellerów:', err),
    // });
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
