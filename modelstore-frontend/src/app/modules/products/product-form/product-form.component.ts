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
import { MultiSelectModule } from 'primeng/multiselect';
import { Category } from '../../../shared/interfaces/category';
import { CategoryMapperService } from '../../../shared/services/category-mapper.service';

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
    MultiSelectModule,
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.css',
})
export class ProductFormComponent implements OnInit {
  categoriesToDropdown: Category[] = [];
  productForm!: FormGroup;
  product: Product;
  state: string;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private categoryMapper: CategoryMapperService,
    private fb: FormBuilder
  ) {
    this.categoriesToDropdown = categoryMapper.categories;
    this.product = config.data.product;
    this.state = config.data.state;
    this.productForm = this.fb.group({
      id: [null],
      name: [null, [Validators.required]],
      brand: [null, [Validators.required]],
      description: [null, [Validators.required]],
      price: [null, [Validators.required]],
      categories: [[this.categoriesToDropdown[0]], [Validators.required]],
      stock: [10, [Validators.required]],
      imageUrl: [null, [Validators.required]],
    });
  }

  ngOnInit() {
    if (this.product !== null) {
      this.productForm.patchValue({
        ...this.product,
        categories: this.product.categories
          .map((categoryId: number) => {
            const category = this.categoryMapper.mapIdToCategory(categoryId);
            if (!category) {
              console.warn(`Nie znaleziono kategorii dla ID: ${categoryId}`);
            }
            return category;
          })
          .filter((category) => category !== null),
      });
    } else {
    }
  }

  submit() {
    let newProduct = this.productForm.getRawValue();
    newProduct.categories = this.productForm.value.categories.map(
      (category: { code: number; name: string }) => {
        return this.categoryMapper.mapCategoryToId(category.name);
      }
    );
    if (this.state === 'add') {
      newProduct.id = Guid.create();
    }
    this.ref.close(newProduct);
  }
}
