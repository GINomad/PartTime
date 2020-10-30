import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AuthService } from '../core/authentication/auth.service';
import { BaseService } from './base.service';
import {HttpOptions} from './http-options';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseApiService extends BaseService {

  constructor(protected authService: AuthService) { 
    super();
  }

  protected get httpOptions(): HttpOptions {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': this.authService.authorizationHeaderValue
      })
    };

    return httpOptions;
  }
}
