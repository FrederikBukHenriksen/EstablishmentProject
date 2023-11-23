import { Injectable, inject } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class HttpInterceptService implements HttpInterceptor {
  public ElectionId: string = '00000000-0000-0000-0000-000000000001';

  constructor() {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const modifiedRequest = request.clone({
      setHeaders: {
        EstablishmentId: this.ElectionId,
      },
    });

    return next.handle(modifiedRequest);
  }
}
