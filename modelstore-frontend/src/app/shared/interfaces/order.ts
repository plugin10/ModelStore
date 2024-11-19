export interface Order {
  id: number;
  userId: number;
  orderDate: Date;
  status: string; // np. "pending", "shipped", "completed"
  items: OrderItem[];
  totalAmount: number;
}

export interface OrderItem {
  productId: number;
  quantity: number;
  price: number; // Cena jednostkowa w momencie zamówienia
}
