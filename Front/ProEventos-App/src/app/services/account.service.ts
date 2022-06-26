import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { User } from '@app/models/Identity/User';
import { UserUpdate } from '@app/models/UserUpdate';
import { environment } from '@environments/environment';
import { map, Observable, ReplaySubject, take } from 'rxjs';

@Injectable()
export class AccountService {
  private currentUserSource = new ReplaySubject<User>(1);
  public currentUser$ = this.currentUserSource.asObservable(); // O $ indica que será um observable

  baseUrl = environment.apiURL + 'api/account/'

  constructor(private http: HttpClient) { }

  public login(model: any): Observable<void> {
    return this.http.post<User>(this.baseUrl + 'login', model)
           .pipe(take(1), map((response) => {
             const user = response;
             if(user) {
                this.setCurrentUser(user)
             }
           }));
  }

  public register(model: any): Observable<void> {
    return this.http.post<User>(this.baseUrl + 'register', model)
           .pipe(take(1), map((response) => {
             const user = response;
             if(user) {
                this.setCurrentUser(user)
             }
           }));
  }

  public logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null as any);
    this.currentUserSource.complete();
  }

  public setCurrentUser(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  getUser(): Observable<UserUpdate> {
    return this.http.get<UserUpdate>(this.baseUrl + 'getUser').pipe(take(1));
  }

  updateUSer(model: UserUpdate): Observable<void>{
    return this.http.put<UserUpdate>(this.baseUrl + 'updateUser', model).pipe(take(1),
    map((user: UserUpdate) => {
      this.setCurrentUser(user);
    }));
  }

  public postUpload(file: File) : Observable<UserUpdate> {
    const fileToUpload = file[0] as File;
    const formData = new FormData();
    formData.append('file', fileToUpload)

    return this.http.post<UserUpdate>(`${this.baseUrl}upload-image`, formData)
          .pipe(take(1));
  }

}
