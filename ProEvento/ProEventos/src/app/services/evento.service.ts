import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators'
import { Evento } from '../models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

constructor(private httpClient: HttpClient) { }

private BASE_URL = "https://localhost:5001/api/evento";

  public getAllEventos(): Observable<Evento[]>{
    return this.httpClient
        .get<Evento[]>(this.BASE_URL)
        .pipe(take(1)); //take serve para fazer o unsubscribe ao final da chamada em questão, se for um Observable
  }

  public getEventoById(idEvento: number): Observable<Evento>{
    return this.httpClient
        .get<Evento>(`${this.BASE_URL}/${idEvento}`)
        .pipe(take(1));
  }

  public getEventoByTema(tema: string): Observable<Evento[]>{
    return this.httpClient
        .get<Evento[]>(`${this.BASE_URL}/${tema}`)
        .pipe(take(1));
  }

  public post(evento: Evento): Observable<Evento>{
    return this.httpClient
        .post<Evento>(this.BASE_URL, evento)
        .pipe(take(1));
  }

  public put(evento: Evento): Observable<Evento>{
    return this.httpClient
        .put<Evento>(`${this.BASE_URL}/${evento.id}`, evento)
        .pipe(take(1));
  }

  public deleteEvento(idEvento: number): Observable<any>{
    return this.httpClient
        .delete(`${this.BASE_URL}/${idEvento}`)
        .pipe(take(1));
  }
}
