import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserAccount } from '../models/UserAccount';
import { Observable } from 'rxjs';
import { tap } from 'rxjs/operators';
import { UserTypeService } from './user-type-service.service';
import { UserAccountResponse } from '../models/UserAccountResponse';
import { AuthService } from './auth.service';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = enviroment.apiUrl;

  constructor(private http:HttpClient, private userTypeService : UserTypeService, private authService : AuthService) { }

  public createAccount(userData: UserAccount): Observable<UserAccount> {
    const token = localStorage.getItem('token')?.toString(); 
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });
    const role = this.userTypeService.getUserType();

    let body = { name: userData.name, surname: userData.surname, email: userData.email, password: userData.password};

    if(role === 'company-owner') {
      return this.http.post<UserAccount>(`${this.apiUrl}/company-owners`, body, { headers, responseType: 'json' });
    }

    return this.http.post<UserAccount>(`${this.apiUrl}/administrators`, body, { headers, responseType: 'json' });
  }

  public getAccounts(offset: number, limit: number, role : string, fullName : string): Observable<UserAccountResponse[]> {
    const token = localStorage.getItem('token')?.toString(); 

    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });
    
    return this.http.get<UserAccountResponse[]>(`${this.apiUrl}/users?offset=${offset}&limit=${limit}&name=${fullName}&role=${role}`, { headers }); 
  }

  public giveHomeOwnerRoleToAdmin() : Observable<string>{
    const token = localStorage.getItem('token')?.toString(); 

    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.put<string>(`${this.apiUrl}/administrators/register-as-home-owner`, {}, { headers, responseType: 'text' as 'json' })
    .pipe(tap(() => {
      this.authService.setRole('admin-home-owner');
    }));
  }

  public giveHomeOwnerRoleToCompanyOwner() : Observable<string>{
    const token = localStorage.getItem('token')?.toString(); 

    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.put<string>(`${this.apiUrl}/company-owners/register-as-home-owner`, {}, { headers, responseType: 'text' as 'json' })
    .pipe(tap(() => {
      this.authService.setRole('company-owner-home-owner');
    }));
  }
}
