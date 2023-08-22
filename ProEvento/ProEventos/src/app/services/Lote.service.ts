import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Lote } from '@app/models/Lote';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs/internal/Observable';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LoteService {

  constructor(private httpClient: HttpClient) { }

  private BASE_URL = environment.apiURL + "lote";

    public getLotesByIdEventoAsync(idEvento: Number): Observable<Lote[]>{
      return this.httpClient
          .get<Lote[]>(`${this.BASE_URL}/${idEvento}`)
          .pipe(take(1)); //take serve para fazer o unsubscribe ao final da chamada em questão, se for um Observable
    }

    public saveLotes(idEvento: number, Lotes: Lote[]): Observable<Lote[]>{
      return this.httpClient
          .put<Lote[]>(`${this.BASE_URL}/${idEvento}`, Lotes)
          .pipe(take(1));
    }

    public deleteLote(idEvento: number, idLote: number): Observable<any>{
      return this.httpClient
          .delete(`${this.BASE_URL}/${idEvento}/${idLote}`)
          .pipe(take(1));
    }
}
