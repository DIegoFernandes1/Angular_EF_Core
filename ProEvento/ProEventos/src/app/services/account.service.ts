import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/Identity/User';
import { UserUpdate } from '@app/models/Identity/UserUpdate';
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

    public getUser(): Observable<UserUpdate> {
      return this.httpClient.get<UserUpdate>(this.BASE_URL + 'getUser').pipe(take(1));
    }

    updateUser(model: UserUpdate): Observable<void>{
      return this.httpClient.put<UserUpdate>(this.BASE_URL + 'updateUser', model).pipe(
        take(1),
        map((user: UserUpdate) => {
          this.setCurrentUser(user)
        }
        )
        )
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

        public postUpload(file: File): Observable<UserUpdate>{

          const fileToUpload = file[0] as File;
          const formData = new FormData();
          formData.append('file', fileToUpload);

          return this.httpClient
          .post<UserUpdate>(`${this.BASE_URL}UploadImage`, formData)
          .pipe(take(1));
        }
      }
