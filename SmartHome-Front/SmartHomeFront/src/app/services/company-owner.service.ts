import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';
import { DeviceRequest } from '../models/DeviceRequest';
import { SmartLamp } from '../models/SmartLamp';
import { Camera } from '../models/Camera';
import { enviroment } from '../models/enviroment';


@Injectable({
  providedIn: 'root'
})
export class CompanyOwnerService {

  constructor(private http: HttpClient) { }

  private apiUrl = enviroment.apiUrl;

  public registerDevice(formData: DeviceRequest): Observable<DeviceRequest> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    let body = { deviceName: formData.deviceName, deviceModel: formData.deviceModel, description: formData.description, photos: formData.photos, deviceType: formData.deviceType };

    return this.http.post<DeviceRequest>(`${this.apiUrl}/sensors`, body, { headers }).pipe(
    catchError(this.handleError)
    );

  }

  public registerSmartLamp(formData: SmartLamp): Observable<SmartLamp> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    let body = { lampName: formData.lampName, lampModel: formData.lampModel, description: formData.description, photos: formData.photos, deviceType: formData.deviceType }; 

    return this.http.post<SmartLamp>(`${this.apiUrl}/smart-lamps`, body, { headers }).pipe(
    catchError(this.handleError)
    );
  }

  public registerCamera(formData: Camera): Observable<Camera> {
    const token = localStorage.getItem('token')?.toString();
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    let body = { cameraName: formData.cameraName, cameraModel: formData.cameraModel, description: formData.description, photos: formData.photos, deviceType: formData.deviceType, canBeUsedIndoors: formData.canBeUsedIndoors, canBeUsedOutdoors: formData.canBeUsedOutdoors, hasMovementDetectionSupport: formData.hasMovementDetectionSupport, hasPersonDetectionSupport: formData.hasPersonDetectionSupport };

    return this.http.post<Camera>(`${this.apiUrl}/cameras`, body, { headers }).pipe(
    catchError(this.handleError)
    );
  }



  private handleError(error: any): Observable<never> {
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
