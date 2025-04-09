import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Notification } from '../models/Notification';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root',
})
export class NotificationService {
  private apiUrl = enviroment.apiUrl;

  constructor(private http: HttpClient) {}

  public getNotifications(
    deviceType: string,
    creationDate: string,
    wasRead: boolean | null
  ): Observable<Notification[]> {
    const token = localStorage.getItem('token')?.toString();

    const headers = new HttpHeaders({
      Authorization: `${token}`,
    });

    let params = `?deviceType=${deviceType}&creationDate=${creationDate}`;
    if (wasRead !== null) {
      params += `&wasRead=${wasRead}`;
    }

    return this.http.get<Notification[]>(
      `${this.apiUrl}/notifications${params}`,
      { headers }
    );
  }
}
