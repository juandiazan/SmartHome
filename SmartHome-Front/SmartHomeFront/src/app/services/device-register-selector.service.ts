import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class DeviceRegisterSelectorService {
  constructor() { }
  private selectedDeviceSubject = new BehaviorSubject<string | null>(null);
  selectedDevice$ = this.selectedDeviceSubject.asObservable();
  
  setSelectedDevice(device: string){
    this.selectedDeviceSubject.next(device);
  }
}
