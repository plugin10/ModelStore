import { Component, OnInit } from '@angular/core';
import {
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  FormBuilder,
  Validators,
} from '@angular/forms';
import { DynamicDialogConfig, DynamicDialogRef } from 'primeng/dynamicdialog';
import { Product } from '../../../shared/interfaces/product';
import { FloatLabelModule } from 'primeng/floatlabel';
import { InputTextModule } from 'primeng/inputtext';
import { DropdownModule } from 'primeng/dropdown';
import { ButtonModule } from 'primeng/button';
import { InputTextareaModule } from 'primeng/inputtextarea';
import { Guid } from 'guid-typescript';

interface CategoryToDropdown {
  name: string;
  code: number;
}

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    FloatLabelModule,
    FormsModule,
    ReactiveFormsModule,
    InputTextModule,
    DropdownModule,
    ButtonModule,
    InputTextareaModule,
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css',
})
export class ProductFormComponent implements OnInit {
  categoriesToDropdown: CategoryToDropdown[] | undefined;
  productForm!: FormGroup;
  product: Product;
  state: string;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private fb: FormBuilder
  ) {
    this.categoriesToDropdown = [
      {
        code: 1,
        name: 'Figurki Fantasy',
      },
      {
        code: 2,
        name: 'Figurki Historyczne',
      },
      {
        code: 3,
        name: 'Akcesoria Modelarskie',
      },
    ];
    this.product = config.data.product;
    this.state = config.data.state;
    this.productForm = this.fb.group({
      id: [null],
      name: [null, [Validators.required]],
      address: [null, [Validators.required]],
      description: [null, [Validators.required]],
      price: [null, [Validators.required]],
      categoryId: [1, [Validators.required]],
      stock: [10, [Validators.required]],
      imageUrl: [null, [Validators.required]],
      scale: ['1:10', [Validators.required]],
      manufacturer: [null, [Validators.required]],
    });
  }

  ngOnInit() {
    if (this.product !== null) {
      this.productForm.patchValue(this.product);
    } else {
    }
  }

  submit() {
    let newProduct = this.productForm.getRawValue();
    if (this.state === 'add') {
      newProduct.id = Guid.create();
    }
    this.ref.close(newProduct);
  }
}
