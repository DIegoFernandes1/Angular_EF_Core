import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { RedeSocial } from '@app/models/RedeSocial';
import { environment } from '@environments/environment';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RedeSocialService {
  baseURL = environment.apiURL + 'RedeSocial'

  constructor(
    private http: HttpClient
    ) { }

    public getRedesSociais(origem: string, id: number): Observable<RedeSocial[]>{
      let url = id === 0
      ? `${this.baseURL}/${origem}`
      : `${this.baseURL}/${origem}/${id}`

      return this.http.get<RedeSocial[]>(url).pipe(take(1))
    }

    public saveRedesSociais(origem: string, id: number, redeSocial: RedeSocial[]): Observable<RedeSocial[]>{
      let url = id === 0
      ? `${this.baseURL}/${origem}`
      : `${this.baseURL}/${origem}/${id}`

      return this.http.post<RedeSocial[]>(url, redeSocial).pipe(take(1))
    }

    public deleteRedeSocial(origem: string, id: number, redeSocialId: number): Observable<any>{
      let url = id === 0
      ? `${this.baseURL}/${origem}/${redeSocialId}`
      : `${this.baseURL}/${origem}/${id}/${redeSocialId}`

      return this.http.delete(url).pipe(take(1))
    }

  }
