import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { enviroment } from '../models/enviroment'; 
import { Observable } from 'rxjs';
import { HomeRoom } from '../models/HomeRoom';
import { Home } from '../models/Home';

@Injectable({
  providedIn: 'root'
})
export class RoomService {
  constructor(private http: HttpClient) { }

  private apiUrl = enviroment.apiUrl;

  public getRoomsOfHome(homeId: string): Observable<HomeRoom[]> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.get<HomeRoom[]>(`${this.apiUrl}/homes/${homeId}/rooms`, { headers });
  }

  public addDeviceToRoom(roomId: string, hardwareId: string): Observable<string> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.put<string>(`${this.apiUrl}/rooms/${roomId}/devices`, { hardwareId }, { headers, responseType: 'text' as 'json' });
  }
}
