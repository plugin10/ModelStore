import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private tokenKey = 'jwtToken';

  private isLoggedInSubject = new BehaviorSubject<boolean>(this.hasToken());
  private userRoleSubject = new BehaviorSubject<string | null>(
    this.decodeUserRole()
  );

  constructor() {}

  private hasToken(): boolean {
    return !!this.getToken();
  }

  private decodeUserRole(): string | null {
    const token = this.getToken();
    if (!token) return null;

    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return payload.role || null;
    } catch (e) {
      console.error('Nie można odczytać roli z tokena', e);
      return null;
    }
  }

  setToken(token: string): void {
    sessionStorage.setItem(this.tokenKey, token);
    this.isLoggedInSubject.next(true);
    this.userRoleSubject.next(this.decodeUserRole());
  }

  getToken(): string | null {
    return sessionStorage.getItem(this.tokenKey);
  }

  clearToken(): void {
    sessionStorage.removeItem(this.tokenKey);
    this.isLoggedInSubject.next(false);
    this.userRoleSubject.next(null);
  }

  getLoggedInStatus() {
    return this.isLoggedInSubject.asObservable();
  }

  getUserRoleStatus() {
    return this.userRoleSubject.asObservable();
  }
}
