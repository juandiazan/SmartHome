import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { Device } from '../models/Device';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class DeviceService {

  constructor(private http: HttpClient) { }

  private apiUrl = enviroment.apiUrl;

  public getDevices(
    offset: number, 
    limit: number,
    deviceName : string, 
    deviceModel : string, 
    companyName : string, 
    deviceType : string): Observable<Device[]> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.get<Device[]>(`${this.apiUrl}/devices?offset=${offset}&limit=${limit}&deviceName=${deviceName}&model=${deviceModel}&companyName=${companyName}&deviceType=${deviceType}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

    public getAllDevices(): Observable<Device[]> {
      const token = localStorage.getItem('token')?.toString();
      const headers = new HttpHeaders({
        'Authorization': `${token}`
      });
      const maxValue = 2147483647;
      const minValue = 1;

      return this.http.get<Device[]>(`${this.apiUrl}/devices?offset=${minValue}&limit=${maxValue}`, { headers }).pipe(
        catchError(error => {
          return this.handleError(error);
        })
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
