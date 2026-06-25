import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { catchError, switchMap, tap } from 'rxjs/operators';

import { BaseService } from '../../shared/base.service';
import { ConfigService } from '../../shared/config.service';
import { UserLogin } from 'src/app/shared/models/user.login';
import { ProfileInfo } from '../profile-info';

interface StoredAuthState {
  email: string;
  clientId?: number;
}

interface AntiforgeryTokenResponse {
  token: string;
}

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService  {

  private readonly stateStorageKey = 'pt.auth.state';

  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  private _clientIdPresenceStatusSource = new BehaviorSubject<boolean>(false);

  authNavStatus$ = this._authNavStatusSource.asObservable();
  clientIdPresenceStatus$ = this._clientIdPresenceStatusSource.asObservable();

  private state: StoredAuthState | null;

  constructor(private http: HttpClient, private configService: ConfigService) {
    super();

    this.state = this.loadState();
    this.publishAuthState();
    this.ensureAntiforgeryToken().subscribe(
      () => {},
      () => {});
  }

  login(userLogin: UserLogin): Observable<any> {
    const request = {
      email: userLogin.email,
      password: userLogin.password
    };

    return this.ensureAntiforgeryToken()
      .pipe(switchMap(() => this.http.post(
          this.configService.authApiURI + '/api/account/login?useCookies=true&useSessionCookies=true',
          request,
          { withCredentials: true })))
      .pipe(
        tap(() => this.storeState(userLogin.email)),
        catchError(this.handleError));
  }

  register(userRegistration: any): Observable<any> {
    const request = {
      email: userRegistration.email,
      password: userRegistration.password
    };

    return this.ensureAntiforgeryToken()
      .pipe(switchMap(() => this.http.post(
          this.configService.authApiURI + '/api/account/register',
          request,
          { withCredentials: true })))
      .pipe(catchError(this.handleError));
  }

  isAuthenticated(): boolean {
    return this.state != null;
  }

  hasClient(): boolean {
    return this.state != null && this.state.clientId != null;
  }

  get name(): string {
    return this.state != null ? this.state.email : '';
  }

  get profile(): ProfileInfo | null {
    if (this.state == null || this.state.clientId == null) {
      return null;
    }

    return <ProfileInfo>{ id: this.state.email, clientId: this.state.clientId };
  }

  setClientId(clientId: number): void {
    if (this.state == null) {
      return;
    }

    this.state = { ...this.state, clientId };
    this.saveState(this.state);
    this._clientIdPresenceStatusSource.next(true);
  }

  signout(): Observable<any> {
    return this.ensureAntiforgeryToken()
      .pipe(switchMap(() => this.http.post(
          this.configService.authApiURI + '/api/account/logout',
          {},
          { withCredentials: true })))
      .pipe(
        tap(() => this.clearState()),
        catchError(error => {
          this.clearState();
          return of(null);
        }));
  }

  private ensureAntiforgeryToken(): Observable<any> {
    return this.http.get<AntiforgeryTokenResponse>(
      this.configService.authApiURI + '/api/antiforgery/token',
      { withCredentials: true })
      .pipe(tap(response => sessionStorage.setItem('pt.xsrf.token', response.token)));
  }

  private storeState(email: string): void {
    this.state = { email };
    this.saveState(this.state);
    this.publishAuthState();
  }

  private loadState(): StoredAuthState | null {
    const rawState = localStorage.getItem(this.stateStorageKey);
    return rawState == null ? null : JSON.parse(rawState) as StoredAuthState;
  }

  private saveState(state: StoredAuthState): void {
    localStorage.setItem(this.stateStorageKey, JSON.stringify(state));
  }

  private clearState(): void {
    this.state = null;
    localStorage.removeItem(this.stateStorageKey);
    this.publishAuthState();
  }

  private publishAuthState(): void {
    this._authNavStatusSource.next(this.isAuthenticated());
    this._clientIdPresenceStatusSource.next(this.hasClient());
  }
}
