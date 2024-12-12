import { OrderElement } from './order-element';

export interface Order {
  id: number;
  userId: number;
  orderDate: Date;
  status: string;
  clientName: string;
  clientEmail: string;
  clientPhone: string;
  shippingAddress: string;
  orderElements: OrderElement[];
}
