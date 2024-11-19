import { ToastType } from "../enums/toast-type";


export class Toast {
  key?: string;
  severity: ToastType;
  summary: string;
  detail: string;
  life?: number;
  sticky?: boolean;

  constructor(
    key: string,
    severity: ToastType,
    summary: string,
    detail: string,
    life: number,
    sticky: boolean
  ) {
    this.key = key;
    this.severity = severity;
    this.summary = summary;
    this.detail = detail;
    this.life = life;
    this.sticky = sticky;
  }
}
