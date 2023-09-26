import { Injectable } from '@angular/core';
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
  constructor() {}

  jwt: string =
    'eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJGcmVkZXJpayIsImp0aSI6IjA4NDVmMzQ5LTQ3ZTQtNDk3Yi05MWFlLTg0YzhjYzUxODgzZSIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6ImFkbWluIiwiZXhwIjoxNjk1NzQ1NjYyLCJpc3MiOiJpc3N1ZXIiLCJhdWQiOiJhdWRpZW5jZSJ9.PIL6O3fWaM_riaoe7TRoGPVaduV1BwJHdm5d9-_qOfc';

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    // const authRequest = req.clone({
    //   setHeaders: {
    //     Authorization: 'Bearer ' + this.jwt,
    //   },
    // });

    return next.handle(req);
  }
}
