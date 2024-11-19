import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import { DynamicDialogRef } from 'primeng/dynamicdialog';
import { InputTextModule } from 'primeng/inputtext';
import { NotificationService } from '../../shared/services/notification.service';
import { ToastType } from '../../shared/enums/toast-type';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, ButtonModule, InputTextModule, FormsModule, ReactiveFormsModule],
  providers: [NotificationService],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  loginForm!: FormGroup;
	blockedlogged = false;
	changePassword = false;
	togglePassword = false;

  constructor(
    public ref: DynamicDialogRef,
    private notificationService: NotificationService,
  ){}

  login(){
    this.ref.close(true);
  }

	ngOnInit() {
		this.loginForm = new FormGroup({
			'login': new FormControl('', Validators.required),
			'password': new FormControl('', Validators.required)
		});
	}
		
	onSubmit() {
		this.blockedlogged = true
    	this.notificationService.showMessage(ToastType.ERROR, "Test", "test");
		// this.authService.login(this.loginForm.value).subscribe({
		// 	next: (data: Token) => {
		// 		if(data.changePassword === true){
		// 			this.blockedlogged = false;
		// 			this.changePassword = data.changePassword;
		// 		}
		// 	},
		// 	error: () => {
		// 		this.blockedlogged = false;
		// 		this.notificationService.showMessage(ToastType.ERROR, this.translationService.getTranslation('global_error'), this.translationService.getTranslation('authorization_invalid_credentials'));
		// 	}
		// });
	}
}