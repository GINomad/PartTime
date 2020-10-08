import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { BaseService } from 'src/app/shared/base.service';
import { ConfigService } from 'src/app/shared/config.service';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService extends BaseService {

  constructor(private http: HttpClient, private configService: ConfigService, private authService: AuthService) {
    super();
  }

  call() {
    const httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        'Authorization': this.authService.authorizationHeaderValue
      })
    };
    return this.http.get(this.configService.clientApiURI + '/identity', httpOptions).pipe(catchError(this.handleError));
  }
}
