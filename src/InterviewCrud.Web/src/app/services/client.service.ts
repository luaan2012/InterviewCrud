import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment.development';
import { Client } from '../models/client';

@Injectable({
  providedIn: 'root',
})
export class ClientService {
  private apiUrl = environment.application.urlClient

  constructor(private http: HttpClient) {
  }

  add(requestClient: any): Observable<any> {
    return this.http.post<any>(this.apiUrl + "addClient", requestClient);
  }

  getAll(): Observable<Client[]> {
    return this.http.get<Client[]>(this.apiUrl + 'listAll');
  }

  delete(id: string): Observable<any> {
    return this.http.delete<any>(this.apiUrl + `delete?guid=${id}`);
  }

  edit(id: string, requestClient: any): Observable<any> {
    return this.http.put<any>(this.apiUrl + `edit?guid=${id}`, requestClient);
  }

  getById(id: string): Observable<Client> {
    return this.http.get<Client>(this.apiUrl + `listbyid?id=${id}`);
  }
}
