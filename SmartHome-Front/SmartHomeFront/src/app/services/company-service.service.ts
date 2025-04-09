import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CompanyResponse } from '../models/CompanyResponse';
import { CompanyCreateRequest } from '../models/CompanyCreateRequest';
import { ModelValidator } from '../models/ModelValidatorResponse';
import { DeviceImporter } from '../models/DeviceImporterResponse';
import { ImportDeviceRequest } from '../models/ImportDeviceRequest';
import { enviroment } from '../models/enviroment';

@Injectable({
  providedIn: 'root'
})
export class CompanyService {
  private apiUrl = enviroment.apiUrl;

  constructor(private http:HttpClient) { }

  public getCompanies(offset: number, limit: number, companyName : string, ownerName : string): Observable<CompanyResponse[]> {
    const token = localStorage.getItem('token')?.toString(); 

    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });
    
    return this.http.get<CompanyResponse[]>(`${this.apiUrl}/companies?offset=${offset}&limit=${limit}&companyName=${companyName}&companyOwnerFullName=${ownerName}`, { headers });
  }

  public getModelValidators(): Observable<ModelValidator[]> {
    const token = localStorage.getItem('token')?.toString(); 

    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });
    
    return this.http.get<ModelValidator[]>(`${this.apiUrl}/model-validators`, { headers });
  }

  public createCompany(companyData: CompanyCreateRequest): Observable<CompanyCreateRequest> {
    const token = localStorage.getItem('token')?.toString(); 
    const headers = new HttpHeaders({
      'Authorization': `${token}`,
      'Content-Type': 'application/json'
    });

    let body = { companyName: companyData.name, logotype: companyData.logo, rut: companyData.rut, modelValidatorId: companyData.modelValidator };
    
    return this.http.post<CompanyCreateRequest>(`${this.apiUrl}/companies`, body, { headers }).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = 'Server error';
        if (error.error instanceof ErrorEvent) {
          errorMessage = error.error.message;
        } else {
          if (typeof error.error === 'string') {
            try {
              const errorResponse = JSON.parse(error.error);
              errorMessage = errorResponse.title || errorMessage;
            } catch (e) {
              errorMessage = error.error;
            }
          } else {
            errorMessage = error.error.title || errorMessage;
          }
        }
        return throwError(() => new Error(errorMessage));
      })
    );
  }

  public getDeviceImporters() : Observable<DeviceImporter[]> {
      const token = localStorage.getItem('token')?.toString(); 

      const headers = new HttpHeaders({
        'Authorization': `${token}`
      });

      return this.http.get<DeviceImporter[]>(`${this.apiUrl}/importers`, { headers });
  }

  public importDevices(importDevices : ImportDeviceRequest) : Observable<ImportDeviceRequest> {
    const token = localStorage.getItem('token')?.toString(); 
    const headers = new HttpHeaders({
      'Authorization': `${token}`
    });

    let body = { deviceImporterId: importDevices.deviceImporterId, filePath: importDevices.filePath };
    
    return this.http.post<ImportDeviceRequest>(`${this.apiUrl}/devices`, body, { headers, responseType: 'text' as 'json' });
  }
}
