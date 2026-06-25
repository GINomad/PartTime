import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class XsrfInterceptor implements HttpInterceptor {

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if (['GET', 'HEAD', 'OPTIONS'].includes(req.method)) {
      return next.handle(req);
    }

    const token = sessionStorage.getItem('pt.xsrf.token');
    if (token == null) {
      return next.handle(req);
    }

    return next.handle(req.clone({
      setHeaders: {
        'X-XSRF-TOKEN': token
      }
    }));
  }
}
