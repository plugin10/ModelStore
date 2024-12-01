import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { MenuItem } from 'primeng/api';
import { MenubarModule } from 'primeng/menubar';
import { BadgeModule } from 'primeng/badge';
import { AvatarModule } from 'primeng/avatar';
import { InputTextModule } from 'primeng/inputtext';
import { CommonModule } from '@angular/common';
import { RippleModule } from 'primeng/ripple';
import { getMenuItems } from './menu-items';
import { DialogService, DynamicDialogRef } from 'primeng/dynamicdialog';
import { LoginComponent } from '../login/login.component';
import { ButtonModule } from 'primeng/button';
import { AuthService } from '../../shared/services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  standalone: true,
  providers: [DialogService],
  imports: [
    RouterModule,
    MenubarModule,
    BadgeModule,
    AvatarModule,
    InputTextModule,
    RippleModule,
    CommonModule,
    ButtonModule,
  ],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss'],
})
export class HeaderComponent implements OnInit, OnDestroy {
  items: MenuItem[] | undefined;
  ref: DynamicDialogRef | undefined;

  isLoggedIn = false;
  userRole: string | null = null;
  private subscriptions: Subscription = new Subscription();

  constructor(
    public router: Router,
    public dialogService: DialogService,
    private authService: AuthService
  ) {}

  ngOnInit() {
    this.items = getMenuItems(this);

    this.subscriptions.add(
      this.authService.getLoggedInStatus().subscribe((status) => {
        this.isLoggedIn = status;
      })
    );

    this.subscriptions.add(
      this.authService.getUserRoleStatus().subscribe((role) => {
        this.userRole = role;
      })
    );
  }

  showLoginDialog() {
    this.ref = this.dialogService.open(LoginComponent, {
      header: 'Logowanie',
      width: '30rem',
      modal: true,
      breakpoints: {
        '960px': '75vw',
        '640px': '95vw',
      },
    });

    this.ref.onClose.subscribe((login: boolean) => {
      if (login) {
        console.log('Użytkownik zalogowany');
      }
    });
  }

  logout() {
    this.authService.clearToken();
    console.log('Użytkownik wylogowany');
  }

  ngOnDestroy() {
    this.subscriptions.unsubscribe();
  }
}
