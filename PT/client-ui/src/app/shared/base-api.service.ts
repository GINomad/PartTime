import { HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BaseService } from './base.service';
import {HttpOptions} from './http-options';

@Injectable({
  providedIn: 'root'
})
export abstract class BaseApiService extends BaseService {

  constructor() { 
    super();
  }

  protected get httpOptions(): HttpOptions {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json'
      }),
      withCredentials: true
    };

    return httpOptions;
  }
}
