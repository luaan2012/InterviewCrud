import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class IdentityService {
  private apiUrl = environment.application.urlIdentity

  constructor(private http: HttpClient) {
  }

  login(emailOrUserName: string, password: string): Observable<any> {
    const credentials = { emailOrUserName, password };
    return this.http.post<any>(this.apiUrl + "signin", credentials);
  }

  register(requestRegister: any): Observable<any> {
    return this.http.post<any>(this.apiUrl + "signup", requestRegister);
  }
}
