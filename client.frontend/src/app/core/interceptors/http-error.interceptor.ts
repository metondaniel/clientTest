import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMsg = 'Erro desconhecido';

        if (error.error instanceof ErrorEvent) {
          errorMsg = `Erro: ${error.error.message}`;
        } else {
          errorMsg = `Erro ${error.status}: ${error.error?.message || error.message}`;
        }

        return throwError(() => error);
      })
    );
  }
}
