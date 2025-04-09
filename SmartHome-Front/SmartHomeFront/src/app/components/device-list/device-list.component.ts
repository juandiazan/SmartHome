import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Device } from '../../models/Device';
import { DeviceItemListComponent } from '../device-item-list/device-item-list.component';
import { DeviceService } from '../../services/device.service';
import { Router } from '@angular/router';
import { GenericFilterComponent } from '../generic-filter/generic-filter.component';
import { PaginationComponent } from '../pagination/pagination.component';

@Component({
  selector: 'app-device-list',
  standalone: true,
  imports: [
    CommonModule,
    DeviceItemListComponent,
    GenericFilterComponent,
    PaginationComponent
  ],
  templateUrl: './device-list.component.html',
  styleUrl: './device-list.component.css'
})


export class DeviceListComponent {
  offset : number = 1;
  limit : number= 10;
  deviceName : string = "";
  deviceModel : string = "";
  companyName : string = "";
  deviceType : string = "";
  noDevices: boolean = false;
  
  setOffset(newOffset: number) {
    this.offset = newOffset;
    this.loadDevices();
  }
  
  setLimit(newLimit: number) {
    this.limit = newLimit;
    this.loadDevices();
  }

  setDeviceName(newName: string) {
    this.deviceName = newName;
    this.loadDevices();
  }

  setDeviceModel(newModel: string) {
    this.deviceModel = newModel;
    this.loadDevices();
  }

  setCompanyName(newName: string) {
    this.companyName = newName;
    this.loadDevices();
  }

  setDeviceType(newType: string) {
    this.deviceType = newType;
    this.loadDevices();
  }

  constructor(
    private deviceService: DeviceService,
    private router: Router
  ) {}

  devices: Device[] = [];

  ngOnInit() {
    this.loadDevices();
  }

  loadDevices() {
    this.deviceService.getDevices(this.offset, this.limit, this.deviceName, this.deviceModel, this.companyName, this.deviceType).subscribe({
      next: devices => {
        this.devices = devices;
        this.noDevices = devices.length === 0;
      },
      error: error => console.error(error)
    });
  }
}
