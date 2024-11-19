import { Component, OnInit } from '@angular/core';
import { Category } from '../../shared/interfaces/category';
import { Product } from '../../shared/interfaces/product';
import { ImageModule } from 'primeng/image';
import { TableModule } from 'primeng/table';
import { ToolbarModule } from 'primeng/toolbar';
import { ButtonModule } from 'primeng/button';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { ProductFormComponent } from './product-form/product-form.component';
import { Guid } from 'guid-typescript';

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
  ],
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.scss'],
})
export class ProductsComponent {
  loginForm!: FormGroup;
  selectedProducts: Product[] = [];
  activateEditButton = false;
  activateDeleteButton = false;

  ref: DynamicDialogRef = new DynamicDialogRef();

  categories: Category[] = [
    {
      id: 1,
      name: 'Figurki Fantasy',
      description: 'Figurki z motywami fantasy i RPG.',
    },
    {
      id: 2,
      name: 'Figurki Historyczne',
      description: 'Figurki z motywami historycznymi i wojskowymi.',
    },
    {
      id: 3,
      name: 'Akcesoria Modelarskie',
      description: 'Farby, pędzle, narzędzia i inne.',
    },
  ];

  products: Product[] = [
    {
      id: Guid.parse('05ff5cf9-3909-220f-44bf-5154020390cb'),
      name: 'Elf Łucznik',
      description: 'Figurka elfa łucznika o szczegółowym wykończeniu.',
      price: 49.99,
      categoryId: 1,
      stock: 15,
      imageUrl: '../../../assets/images/figurka.jpg',
      scale: '1:32',
      manufacturer: 'FantasyModels',
    },
    {
      id: Guid.parse('2856988f-4c4c-27a9-a63c-0b19fac95ecc'),
      name: 'Rycerz Templariusz',
      description: 'Figurka rycerza templariusza z epoki krucjat.',
      price: 79.99,
      categoryId: 2,
      stock: 10,
      imageUrl: '../../../assets/images/figurka.jpg',
      scale: '1:35',
      manufacturer: 'HistoricFigures',
    },
    {
      id: Guid.parse('ac46f09c-ab1b-23ce-ef93-d2a6fa835a48'),
      name: 'Farby Akrylowe Set 12',
      description: 'Zestaw 12 kolorów farb akrylowych dla modelarzy.',
      price: 25.99,
      categoryId: 3,
      stock: 25,
      imageUrl: '../../../assets/images/figurka.jpg',
      scale: 'N/A',
      manufacturer: 'ModelTools',
    },
  ];

  constructor(private dialogService: DialogService) {}

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
        // Sprawdź, czy produkt istnieje na liście
        const index = this.products.findIndex((p) => p.id.equals(product.id));

        console.log(index);

        if (index !== -1) {
          // Jeśli istnieje, podmień
          console.log('działa');
          this.products[index] = product;
          this.products = [...this.products];
        } else {
          // Jeśli nie istnieje, dodaj
          this.products = [...this.products, product];
        }

        console.log(this.products);
      }
    });
  }

  rowSelectChanged() {
    this.activateDeleteButton = this.selectedProducts.length > 0;
    this.activateEditButton = this.selectedProducts.length === 1;
  }
}
