import { Injectable, inject } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { SessionStorageService } from '../session-storage/session-storage.service';

@Injectable({
  providedIn: 'root',
})
export class HttpInterceptService implements HttpInterceptor {
  private sessionStorageService = inject(SessionStorageService);
  private ActiveEstablishmentId: string =
    this.sessionStorageService.getActiveEstablishment() == null
      ? ''
      : this.sessionStorageService.getActiveEstablishment()!;

  constructor() {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // const modifiedRequest = request.clone({
    //   setHeaders: {
    //     EstablishmentId: this.ActiveEstablishmentId,
    //   },
    // });

    return next.handle(request);
  }
}
