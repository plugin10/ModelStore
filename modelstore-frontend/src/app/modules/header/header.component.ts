import { Component } from '@angular/core';
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
  styleUrl: './header.component.scss',
})
export class HeaderComponent {
  items: MenuItem[] | undefined;
  ref: DynamicDialogRef | undefined;

  constructor(public router: Router, public dialogService: DialogService) {}

  ngOnInit() {
    this.items = getMenuItems(this);
  }

  showTasks() {}

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
      }
    });
  }
}
