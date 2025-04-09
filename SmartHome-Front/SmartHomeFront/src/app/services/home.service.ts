import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, map, Observable, throwError } from 'rxjs';
import { ListDevice } from '../models/ListDevice';
import { ListHome } from '../models/ListHome';
import { HomeMember } from '../models/HomeMember';
import { MemberToHome } from '../models/MemberToHome';
import { Home } from '../models/Home';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class HomeService {

  constructor(private http: HttpClient) { }

  private apiUrl = enviroment.apiUrl;

  public createHome(userData: Home): Observable<Home> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    let body = { ownerEmail: userData.ownerEmail, mainStreet: userData.mainStreet, doorNumber: userData.doorNumber, latitude: userData.latitude, longitude: userData.longitude, maxAmountOfMembers: userData.maxAmountOfMembers, alias: userData.alias };

    return this.http.post<Home>(`${this.apiUrl}/homes`, body, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  public addMemberToHome(homeId: string, formData: MemberToHome): Observable<MemberToHome> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    let body = { emailOfNewMember: formData.emailOfNewMember, canAddDeviceToHome: formData.canAddDeviceToHome, canSeeDevicesOfHome: formData.canSeeDevicesOfHome, canChangeAliasOfDevices: formData.canChangeAliasOfDevices };

    return this.http.put<MemberToHome>(`${this.apiUrl}/homes/${homeId}/members`, body, { headers, responseType: 'text' as 'json' }).pipe(
      catchError(this.handleError)
    );
  }


  public getHomes(): Observable<ListHome[]> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.get<ListHome[]>(`${this.apiUrl}/homes`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  public getHomesIOwn(): Observable<ListHome[]> {
    return this.getHomes().pipe(
      map((homes: ListHome[]) => {
        return homes.filter(home => home.isOwner);
      })
    );
  }


  public getHomeMembers(homeId: string): Observable<HomeMember[]> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.get<HomeMember[]>(`${this.apiUrl}/homes/${homeId}/members`, { headers }).pipe(
      catchError(this.handleError)
    );
  }

  public getHomeDevices(homeId: string, room: string): Observable<ListDevice[]> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.get<ListDevice[]>(`${this.apiUrl}/homes/${homeId}/devices?room=${room}`, { headers }).pipe(
      catchError(this.handleError)
    );
  }


  public getHomesWithListDevicesPermission(): Observable<ListHome[]> {
    return this.getHomes().pipe(
      map((homes: ListHome[]) => {
        return homes.filter(home => Array.isArray(home.permissions) && home.permissions.includes('list-devices-of-specific-home'));
      })
    );
  }

  public getHomesWithModifyDeviceNamePermission(): Observable<ListHome[]> {
    return this.getHomes().pipe(
      map((homes: ListHome[]) => {
        return homes.filter(home => Array.isArray(home.permissions) && home.permissions.includes('change-alias-of-specific-device'));
      })
    );
  }

  public updateMemberNotificationPermissions(notificationsEnabled: boolean, homeId: string, memberId: string): Observable<void> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.put<void>(`${this.apiUrl}/homes/${homeId}/notifications`, { notificationsEnabled, memberId }, { headers, responseType: 'text' as 'json'  }).pipe(
      catchError(this.handleError)
    );
  }

  public addDeviceToHome(homeId: string, alias: string, deviceId: string): Observable<void> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.put<void>(`${this.apiUrl}/homes/${homeId}/devices`, { deviceId, alias }, { headers, responseType: 'text' as 'json' }).pipe(
      catchError(this.handleError)
    );
  }

  public addRoomToHome(homeId: string, roomName: string): Observable<void> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    return this.http.put<void>(`${this.apiUrl}/homes/${homeId}/rooms`, { roomName }, { headers, responseType: 'text' as 'json' }).pipe(
      catchError(this.handleError)
      );
  }

  public getHomesWithAddDevicePermission(): Observable<ListHome[]> {
    return this.getHomes().pipe(
      map((homes: ListHome[]) => {
        return homes.filter(home => Array.isArray(home.permissions) && home.permissions.includes('add-device-to-specific-home'));
      })
    );
  }

  private handleError(error: HttpErrorResponse): Observable<never> {
    let errorMessage = 'Unknown error!';
    if (error.error instanceof ErrorEvent) {
      errorMessage = `Error: ${error.error.message}`;
    } else {
      errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
    }
    console.error(errorMessage);
    return throwError(() => new Error(errorMessage));
  }
}
