import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Evento } from '../models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

constructor(private httpClient: HttpClient) { }

private BASE_URL = "https://localhost:5001/api/evento";

  public getAllEventos(): Observable<Evento[]>{
    return this.httpClient.get<Evento[]>(this.BASE_URL);
  }

  public getEventoById(idEvento: number): Observable<Evento>{
    return this.httpClient.get<Evento>(`${this.BASE_URL}/${idEvento}`);
  }

  public getEventoByTema(tema: string): Observable<Evento[]>{
    return this.httpClient.get<Evento[]>(`${this.BASE_URL}/${tema}`);
  }
}
