import { CreateOrderElementRequest } from './create-order-element-request';

export interface CreateOrderRequest {
  userId?: number;
  clientName?: string;
  clientEmail?: string;
  clientPhone?: string;
  shippingAddress: string;
  elements: CreateOrderElementRequest[];
}
