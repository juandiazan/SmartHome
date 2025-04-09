import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { HomeDevice } from '../models/HomeDevice';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class HomeDeviceService {

  constructor(private http: HttpClient) { }

  private apiUrl = enviroment.apiUrl;


  public modifyHomeDeviceAlias(hardwareId: string, alias: string): Observable<HomeDevice> {
    const token = localStorage.getItem('token')?.toString();
    const headers = {
      'Authorization': `${token}`
    };

    let body = { hardwareId: hardwareId, alias: alias };

    return this.http.put<HomeDevice>(`${this.apiUrl}/home-devices/${hardwareId}/alias`, body, { headers, responseType: 'text' as 'json' }).pipe(
      catchError(this.handleError)
    );
  }

  private handleError(error: any) {
    let errorMessage = 'Unknown error';
    if (error.error) {
      try {
        const errorObj = typeof error.error === 'string' ? JSON.parse(error.error) : error.error;
        errorMessage = errorObj.title || errorMessage;
      } catch (e) {
        console.error('Error parsing error body as JSON', e);
      }
    }

    return throwError(() => new Error(errorMessage));
  }
}
