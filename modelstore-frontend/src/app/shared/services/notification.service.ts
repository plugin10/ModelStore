import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';
import { ToastType } from '../enums/toast-type';
import { Toast } from '../models/toast.model';

@Injectable({
  providedIn: 'root'
})

export class NotificationService {
  private messageQueue: Toast[] = [];
  messageLife = 3000;
  constructor(private messageService: MessageService) { }

  public showMessage(type: ToastType, header: string, body: string, life = this.messageLife) {
    const msg: Toast = {
      key: 'toast',
      severity: type,
      summary: header, 
      detail: body,
      life: life,
    };
    this.messageService.add(msg);
  }

  public addMessage(type: ToastType, summary: string, detail: string, life = this.messageLife) {
    const msg: Toast = {
      key: 'toast',
      severity: type,
      summary: summary,
      detail: detail,
      life: life,
    };
    this.messageQueue.push(msg);
  }

  public flushMessages() {
    for (const msg of this.messageQueue) {
      this.messageService.add(msg);
    }
    this.messageQueue = [];
  }
}
