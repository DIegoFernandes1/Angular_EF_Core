import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PaginatedResult } from '@app/models/Pagination';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators'
import { Evento } from '../models/Evento';

@Injectable({
  providedIn: 'root'
})
export class EventoService {

  constructor(private httpClient: HttpClient) { }

  private BASE_URL = environment.apiURL + "evento";

  public getAllEventos(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Evento[]>> {

    const paginatedResult: PaginatedResult<Evento[]> = new PaginatedResult<Evento[]>();

    let params = new HttpParams;

    if(page != null && itemsPerPage != null){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if(term != null && term != ''){
      params = params.append('term', term);
    }

    return this.httpClient
    .get<Evento[]>(this.BASE_URL, {observe: 'response', params})
    .pipe(
      take(1), //take serve para fazer o unsubscribe ao final da chamada em questão, se for um Observable
      map((response) => {

        paginatedResult.result = response.body;

        if(response.headers.has('Pagination')){
          paginatedResult.pagination = JSON.parse(response.headers.get('Pagination'));
        }

        return paginatedResult;
      }
      ));
    }

    public getEventoById(idEvento: number): Observable<Evento>{
      return this.httpClient
      .get<Evento>(`${this.BASE_URL}/${idEvento}`)
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

    public postUpload(idEvento: number, file: File): Observable<Evento>{

      const fileToUpload = file[0] as File;
      const formData = new FormData();
      formData.append('file', fileToUpload);

      return this.httpClient
      .post<Evento>(`${this.BASE_URL}/UploadImage/${idEvento}`, formData)
      .pipe(take(1));
    }
  }
