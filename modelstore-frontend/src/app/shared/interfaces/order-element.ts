import { Guid } from 'guid-typescript';

export interface OrderElement {
  id: number;
  productId: Guid;
  orderId: number;
  quantity: number;
  price: number;
}
