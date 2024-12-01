import { Component } from '@angular/core';
import { Product } from '../../shared/interfaces/product';
import { ImageModule } from 'primeng/image';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ProductFormComponent } from './product-form/product-form.component';
import { DataViewModule } from 'primeng/dataview';
import { Guid } from 'guid-typescript';
import { TagModule } from 'primeng/tag';
import { CommonModule } from '@angular/common';
import { RatingModule } from 'primeng/rating';
import { ApiService } from '../../shared/services/api.service';
import { HttpClientModule } from '@angular/common/http';
import { AuthService } from '../../shared/services/auth.service';

@Component({
  selector: 'app-products',
  standalone: true,
  providers: [DialogService],
  imports: [
    TableModule,
    ImageModule,
    ToolbarModule,
    ButtonModule,
    FormsModule,
    ReactiveFormsModule,
    DataViewModule,
    TagModule,
    CommonModule,
    RatingModule,
    HttpClientModule,
  ],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class ProductsComponent {
  loginForm!: FormGroup;
  selectedProducts: Product[] = [];
  activateEditButton = false;
  activateDeleteButton = false;
  testApiProducts: any[] = [];
  isLoggedIn = false;
  userRole: string | null = null;

  ref: DynamicDialogRef = new DynamicDialogRef();

  products: Product[] = [];

  constructor(
    private dialogService: DialogService,
    private apiService: ApiService,
    private authService: AuthService
  ) {}

  ngOnInit(): void {
    this.loadProducts();
    this.isLoggedIn = this.authService.isLoggedIn();
    this.userRole = this.authService.getUserRole();
    console.log(this.userRole);
  }

  showProductForm(product: Product | null, state: any) {
    const header = 'contractor_client_register_form_header_' + state;
    this.ref = this.dialogService.open(ProductFormComponent, {
      header: state === 'add' ? 'Dodaj nowy produkt' : 'Eytuj produkt',
      width: '50%',
      height: '85%',
      style: { 'max-width': '900px' },
      contentStyle: { 'md:max-height': '700px', overflow: 'auto' },
      breakpoints: {
        '1500px': '70vw',
        '960px': '80vw',
        '640px': '95vw',
      },
      baseZIndex: 10000,
      data: {
        product: product,
        state: state,
      },
    });

    this.ref.onClose.subscribe((product: Product) => {
      if (product) {
        if (state === 'add') {
          this.apiService.createProduct(product).subscribe({
            next: () => {
              this.loadProducts();
            },
            error: (err) => {
              console.error('Błąd podczas dodawania produktu:', err);
            },
          });
        } else if (state === 'edit') {
          console.log(product);
          this.apiService.updateProduct(product.id, product).subscribe({
            next: () => {
              this.loadProducts();
            },
            error: (err) => {
              console.error('Błąd podczas aktualizacji produktu:', err);
            },
          });
        }
      }
    });
  }

  rowSelectChanged() {
    this.activateDeleteButton = this.selectedProducts.length > 0;
    this.activateEditButton = this.selectedProducts.length === 1;
  }

  loadProducts(): void {
    this.apiService.getProducts().subscribe({
      next: (data: any) => {
        this.products = data.items.map((item: any) => ({
          id: Guid.parse(item.id),
          name: item.name,
          brand: item.brand,
          slug: item.slug,
          rating: item.rating,
          price: item.price,
          stock: item.stock,
          categories: item.categories,
          description: item.description,
          imageUrl: item.imageUrl || null,
        }));
      },
      error: (err) => {
        console.error('Błąd podczas pobierania produktów:', err);
      },
    });
  }

  getSeverity(product: any) {
    if (product.stock === 0) {
      return 'danger'; // OUTOFSTOCK
    } else if (product.stock > 0 && product.stock <= 8) {
      return 'warning'; // LOWSTOCK
    } else {
      return 'success'; // INSTOCK
    }
  }
}
