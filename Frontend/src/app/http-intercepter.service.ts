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
export class HttpIntercepterService implements HttpInterceptor {
  constructor() {}

  jwt: string =
    'bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsImV4cCI6MTY5NTc0MzQ1N30.3KW5326y-kxpqzXPTPblwJhwG9-wGndt3OFDbNo8a_Q';

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    req.headers.append('Authorization', this.jwt);
    return next.handle(req);
  }
}
