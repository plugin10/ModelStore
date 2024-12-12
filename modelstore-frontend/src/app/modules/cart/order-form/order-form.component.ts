import { Component } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { FloatLabelModule } from 'primeng/floatlabel';
import { DynamicDialogRef, DynamicDialogConfig } from 'primeng/dynamicdialog';
import { ButtonModule } from 'primeng/button';
import { InputTextModule } from 'primeng/inputtext';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [
    FormsModule,
    ReactiveFormsModule,
    ButtonModule,
    InputTextModule,
    FloatLabelModule,
  ],
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.css'],
})
export class OrderFormComponent {
  orderForm!: FormGroup;

  constructor(
    public ref: DynamicDialogRef,
    public config: DynamicDialogConfig,
    private fb: FormBuilder
  ) {
    this.orderForm = this.fb.group({
      clientName: [null, [Validators.required]],
      clientEmail: [null, [Validators.required, Validators.email]],
      clientPhone: [null, [Validators.required, Validators.pattern(/^\d{9}$/)]],
      shippingAddress: [null, [Validators.required]],
    });
  }

  submit() {
    if (this.orderForm.valid) {
      this.ref.close(this.orderForm.value);
    }
  }

  cancel() {
    this.ref.close(null);
  }
}
