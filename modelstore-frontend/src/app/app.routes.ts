import { Routes } from '@angular/router';
import { HomeComponent } from './modules/home/home.component';
import { ProductsComponent } from './modules/products/products.component';
import { CartComponent } from './modules/cart/cart.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'products', component: ProductsComponent },
  { path: 'cart', component: CartComponent },
  { path: '**', redirectTo: '' },
];
