import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError } from 'rxjs/operators';
import { UserManager, UserManagerSettings, User, Profile  } from 'oidc-client';
import { BehaviorSubject } from 'rxjs'; 

import { BaseService } from "../../shared/base.service";
import { ConfigService } from '../../shared/config.service';
import { UserLogin } from 'src/app/shared/models/user.login';
import { ProfileInfo } from '../profile-info';

@Injectable({
  providedIn: 'root'
})
export class AuthService extends BaseService  {

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  private _clientIdPresenceStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();
  clientIdPresenceStatus$ = this._clientIdPresenceStatusSource.asObservable();

  private manager = new UserManager(getClientSettings());
  private user: User | null;

  constructor(private http: HttpClient, private configService: ConfigService) { 
    super();     
    
    this.manager.getUser().then(user => { 
      this.user = user;     
      this._authNavStatusSource.next(this.isAuthenticated());
      this._clientIdPresenceStatusSource.next(this.hasClient());
    });
  }

  login() { 
    return this.manager.signinRedirect();   
  }

  async completeAuthentication() {
      this.user = await this.manager.signinRedirectCallback();
      this._authNavStatusSource.next(this.isAuthenticated());  
      this._clientIdPresenceStatusSource.next(this.hasClient());
  }  

  register(userRegistration: any) {    
    return this.http.post(this.configService.authApiURI + '/api/account', userRegistration).pipe(catchError(this.handleError));
  }

  isAuthenticated(): boolean {
    return this.user != null && !this.user.expired;
  }

  hasClient(): boolean {
    return this.user.profile != null && this.user.profile.client_user_id != null && this.user.profile.client_user_id != "";
  }

  get authorizationHeaderValue(): string {
    return `${this.user.token_type} ${this.user.access_token}`;
  }

  get name(): string {
    return this.user != null ? this.user.profile.name : '';
  }

  get profile(): ProfileInfo {
    var profile = (this.user != null && this.user.profile != null) ? this.user.profile : null;
    if(profile == null)
    {
      return null;
    }
    return <ProfileInfo>{id: profile.sub, clientId: profile.client_user_id}; 
  }

  setClientId(clientId: number): void{
    this.user.profile.client_user_id = clientId;
    this._clientIdPresenceStatusSource.next(true);
  }

  async signout() {
    await this.manager.signoutRedirect();
  }
}

export function getClientSettings(): UserManagerSettings {
  return {
      authority: 'https://localhost:44331/',
      client_id: 'pt_spa',
      redirect_uri: 'http://localhost:4200/auth-callback',
      post_logout_redirect_uri: 'http://localhost:4200/',
      response_type:"id_token token",
      scope:"openid profile email client.all",
      filterProtocolClaims: true,
      loadUserInfo: true,
      automaticSilentRenew: true,
      silent_redirect_uri: 'http://localhost:4200/silent-refresh.html'
  };
}