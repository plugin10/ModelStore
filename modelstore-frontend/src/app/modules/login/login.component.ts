import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';
import { NotificationService } from '../../shared/services/notification.service';
import { ToastType } from '../../shared/enums/toast-type';
import { ApiService } from '../../shared/services/api.service';
import { AuthService } from '../../shared/services/auth.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    CommonModule,
    ButtonModule,
    InputTextModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  providers: [NotificationService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  loginForm!: FormGroup;
  blockedlogged = false;
  changePassword = false;
  togglePassword = false;

  constructor(
    public ref: DynamicDialogRef,
    private apiService: ApiService,
    private authService: AuthService,
    private notificationService: NotificationService
  ) {}

  login() {
    this.ref.close(true);
  }

  ngOnInit() {
    this.loginForm = new FormGroup({
      email: new FormControl('', Validators.required),
      password: new FormControl('', Validators.required),
    });
  }

  onSubmit() {
    this.blockedlogged = true;

    // console.log(this.loginForm.value);

    this.apiService.login(this.loginForm.value).subscribe({
      next: (data) => {
        this.authService.setToken(data.token);
        console.log(this.authService.getToken());
        this.notificationService.showMessage(
          ToastType.SUCCESS,
          'Zalogowano pomyślnie',
          ''
        );
        this.blockedlogged = false;
      },
      error: () => {
        this.notificationService.showMessage(
          ToastType.ERROR,
          'Błąd logowania',
          'Nieprawidłowy login lub hasło'
        );
        this.blockedlogged = false;
      },
    });
  }
}
