import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { BaseApiService } from 'src/app/shared/base-api.service';
import { ConfigService } from 'src/app/shared/config.service';
import { Profile } from 'src/app/shared/models/profile/profile';
import { AuthService } from '../authentication/auth.service';

@Injectable({
  providedIn: 'root'
})
export class ProfileService extends BaseApiService {

  constructor(private http: HttpClient, private configService: ConfigService, authService: AuthService) {
    super(authService);
  }

  createProfile(profile: Profile):Observable<Profile> {
    return this.http.post<Profile>(this.configService.clientApiURI + '/client', profile, this.httpOptions).pipe(catchError(this.handleError));
  }
  
  call() {
    return this.http.get(this.configService.clientApiURI + '/identity', this.httpOptions).pipe(catchError(this.handleError));
  }

  getProfile(id: number): Observable<Profile> {
    return this.http.get<Profile>(this.configService.clientApiURI + `/client/${id}`, this.httpOptions).pipe(catchError(this.handleError));
  }

  
}
