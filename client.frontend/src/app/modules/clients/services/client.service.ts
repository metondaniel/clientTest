import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Client } from '../client.model';

@Injectable({ providedIn: 'root' })
export class ClientsService {
  private apiUrl = 'https://localhost:44330/';

  constructor(private http: HttpClient) { }

  getClients(filter?: any): Observable<Client[]> {
    return this.http.get<Client[]>(this.apiUrl, { params: filter });
  }

  createClient(client: Client): Observable<Client> {
    return this.http.post<Client>(this.apiUrl, client);
  }

  updateClient(id: string, client: Client): Observable<Client> {
    return this.http.put<Client>(`${this.apiUrl}/${id}`, client);
  }

  deleteClient(id: string): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
