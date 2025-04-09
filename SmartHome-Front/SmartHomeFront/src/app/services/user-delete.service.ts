import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { UserAccountResponse } from '../models/UserAccountResponse';
import { Router } from '@angular/router';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class UserDeleteService {
  private apiUrl = enviroment.apiUrl;
  private userToBeDeleted! : UserAccountResponse;

  constructor(private http:HttpClient, private router: Router) { }

  public setUserToBeDeleted(user: UserAccountResponse){
    this.userToBeDeleted = user;
  }

  public getUserToBeDeleted(): UserAccountResponse | null{
    return this.userToBeDeleted;
  }

  public deleteUser(user: UserAccountResponse): void {
    const token = localStorage.getItem('token')?.toString(); 
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    if (user.role === 'administrator') {
      this.http.delete(`${this.apiUrl}/administrators/${user.id}`, { headers }).subscribe({
        next: () => {
          alert('User deleted successfully');
          this.router.navigate(['/delete-users']);
        },
        error: (error) => {
          alert('User has an active session');
        }
      });
    } else {
      alert('User is not an administrator');
    }
    
    this.router.navigate(['/delete-users']);
  }

}
