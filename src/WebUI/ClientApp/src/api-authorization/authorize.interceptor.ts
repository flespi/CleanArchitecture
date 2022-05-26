import { Inject, Injectable } from '@angular/core';
import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthorizeService } from './authorize.service';
import { mergeMap } from 'rxjs/operators';
import { API_BASE_URL } from 'src/app/web-api-client';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeInterceptor implements HttpInterceptor {
  constructor(private authorize: AuthorizeService, @Inject(API_BASE_URL) private baseUrl?: string) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return this.authorize.getAccessToken()
      .pipe(mergeMap(token => this.processRequestWithToken(token, req, next)));
  }

  // Checks if there is an access_token available in the authorize service
  // and adds it to the request in case it's targeted at the same origin as the
  // single page application.
  private processRequestWithToken(token: string | null, req: HttpRequest<any>, next: HttpHandler) {
    if (!!token && this.isWebApiUrl(req)) {
      req = req.clone({
        setHeaders: {
          Authorization: `Bearer ${token}`
        }
      });
    }

    return next.handle(req);
  }

  private isWebApiUrl(req: any) {
    return req.url.startsWith(`${this.baseUrl}/`);
  }
}
