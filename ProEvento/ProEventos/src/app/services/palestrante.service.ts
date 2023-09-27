import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Palestrante } from '@app/models/Palestrante';
import { PaginatedResult } from '@app/models/Pagination';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { take, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class PalestranteService {

  constructor(private httpClient: HttpClient) { }

  private BASE_URL = environment.apiURL + "palestrante";

  public getAllPalestrantes(page?: number, itemsPerPage?: number, term?: string): Observable<PaginatedResult<Palestrante[]>> {

    const paginatedResult: PaginatedResult<Palestrante[]> = new PaginatedResult<Palestrante[]>();

    let params = new HttpParams;

    if(page != null && itemsPerPage != null){
      params = params.append('pageNumber', page.toString());
      params = params.append('pageSize', itemsPerPage.toString());
    }

    if(term != null && term != ''){
      params = params.append('term', term);
    }

    return this.httpClient
    .get<Palestrante[]>(this.BASE_URL + '/all', {observe: 'response', params})
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

    public getPalestrante(): Observable<Palestrante>{
      return this.httpClient
      .get<Palestrante>(`${this.BASE_URL}`)
      .pipe(take(1));
    }

    public post(): Observable<Palestrante>{
      return this.httpClient
      .post<Palestrante>(this.BASE_URL, {} as Palestrante)
      .pipe(take(1));
    }

    public put(palestrante: Palestrante): Observable<Palestrante>{
      return this.httpClient
      .put<Palestrante>(`${this.BASE_URL}`, palestrante)
      .pipe(take(1));
    }
  }
