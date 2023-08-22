import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/Identity/User';
import { environment } from '@environments/environment';
import { Observable, ReplaySubject } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

constructor(private httpClient: HttpClient) { }

  private BASE_URL = environment.apiURL + "Account/";
  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable();

  public login(model: any): Observable<void>{
    return this.httpClient.post<User>(this.BASE_URL + 'Login', model).pipe(
      take(1),
      map((response: User) => {
        const user = response;

        if(user){
            this.setCurrentUser(user);
        }
      })
    );
  }

  public logout(): void{
    localStorage.removeItem('user');
    this.currentUserSource.next(null as any);
    //this.currentUserSource.complete();
  }

  public setCurrentUser(user: any): void{
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  public Register(model: any): Observable<void>{
    return this.httpClient.post<User>(this.BASE_URL + 'RegisterUser', model).pipe(
      take(1),
      map((reposnse: User) => {
        const user = reposnse;

        if(user){
          this.setCurrentUser(user);
        }
      })
    );
  }
}
