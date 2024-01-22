import { Injectable } from "@angular/core";
import {
  HttpEvent,
  HttpInterceptor,
  HttpHandler,
  HttpRequest,
} from "@angular/common/http";
import { Observable } from "rxjs";
import { getUser } from "../helpers/storage";

@Injectable({ providedIn: "root" })
export class TokenInterceptor implements HttpInterceptor {
  constructor() {}

  intercept(
    req: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    const token = getUser()?.accessToken;

    const request = req.clone({
      setHeaders: {
        ...(token ? { Authorization: `Bearer ${token}` } : {}),
      },
    });
    return next.handle(request);
  }
}
