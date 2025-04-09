import { Injectable} from '@angular/core';
import { HttpClient, HttpHeaders  } from '@angular/common/http';
import { Observable } from 'rxjs';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class DeviceTypesService {
  private readonly apiUrl = enviroment.apiUrl;

  constructor(private http: HttpClient) {}

  getAllDeviceTypes(): Observable<string[]> {
    const token = localStorage.getItem('token')?.toString(); 

    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.get<string[]>(`${this.apiUrl}/device-types`, { headers });
  }
}
