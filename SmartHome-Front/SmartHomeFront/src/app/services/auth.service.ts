import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, BehaviorSubject} from 'rxjs';
import { Login } from '../models/Login';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private ApiUrl = enviroment.apiUrl;

  private roleSubject = new BehaviorSubject<string | null>(localStorage.getItem('userRole') || null);
  role$ = this.roleSubject.asObservable();

  constructor(private http: HttpClient) { }

  login(loginData : Login): Observable<string> {
    const body = { email: loginData.email, password: loginData.password };
    return this.http.post<string>(`${this.ApiUrl}/sessions`, body, { responseType: 'text' as 'json' });
  }

  fetchUserRole(): Observable<string> {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders().set('Authorization', `${token}`);
    return this.http.get<string>(`${this.ApiUrl}/sessions/user-role`, { headers, responseType: 'text' as 'json' });
  }

  setRole(role: string) {
    this.roleSubject.next(role);
    localStorage.setItem('userRole', role);
  }

  getRole(): string | null {
    return this.roleSubject.getValue();  }
}
