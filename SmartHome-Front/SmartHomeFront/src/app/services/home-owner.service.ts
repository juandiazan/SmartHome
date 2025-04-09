import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HomeOwner } from '../models/HomeOwner';
import { enviroment } from '../models/enviroment';

@Injectable({
  providedIn: 'root',
})
export class HomeOwnerService {
  private apiUrl = enviroment.apiUrl;

  constructor(private http: HttpClient) {}

  public createHomeOwnerAccount(userData: HomeOwner): Observable<HomeOwner> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      Authorization: `${token}`,
    });

    let body = {
      profilePicture: userData.profilePicture,
      name: userData.name,
      surname: userData.surname,
      email: userData.email,
      password: userData.password,
    };

    return this.http
      .post<HomeOwner>(`${this.apiUrl}/home-owners`, body, { headers })
      .pipe(catchError(this.handleError));
  }

  public getHomeIdByToken(): Observable<string> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      Authorization: `${token}`,
    });

    return this.http
      .get<string>(`${this.apiUrl}/home-owners/ownedHomeId`, {
        headers,
        responseType: 'text' as 'json',
      })
      .pipe(catchError(this.handleError));
  }

  private handleError(error: any) {
    let errorMessage = 'Unknown error';
    if (error.error) {
      try {
        const errorObj =
          typeof error.error === 'string'
            ? JSON.parse(error.error)
            : error.error;
        errorMessage = errorObj.title || errorMessage;
      } catch (e) {
        console.error('Error parsing error body as JSON', e);
      }
    }

    return throwError(() => new Error(errorMessage));
  }
}
