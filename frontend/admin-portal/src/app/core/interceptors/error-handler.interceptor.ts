import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class ErrorHandlerInterceptor implements HttpInterceptor {
  intercept(
    request: HttpRequest<unknown>,
    next: HttpHandler,
  ): Observable<HttpEvent<unknown>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let message = 'something went wrong';

        if (error.status === 401) {
          message = 'User should login with credentials';
        } else if (error.status === 403) {
          message = 'User is not allowed to perform this operation';
        } else if (error.status === 404) {
          message = 'The item was not found';
        }

        return throwError(() => new Error(message));
      }),
    );
  }
}
