import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private apiUrl = environment.application.urlClient

  constructor(private http: HttpClient) {
  }

  add(requestClient: any): Observable<any> {
    return this.http.post<any>(this.apiUrl + "signin", requestClient);
  }

  delete(id: string): Observable<any> {
    return this.http.post<any>(this.apiUrl + "signup", id);
  }

  edit(id: string, requestClient: any): Observable<any> {
    return this.http.post<any>(this.apiUrl + "signup", requestClient);
  }
}
