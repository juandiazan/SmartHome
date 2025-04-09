import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RegisterDeviceCameraFormComponent } from '../../../components/register-device-camera-form/register-device-camera-form.component';
import { RegisterDeviceMovementSensorComponent } from '../../../components/register-device-movement-sensor/register-device-movement-sensor.component';
import { RegisterDeviceWindowSensorFormComponent } from '../../../components/register-device-window-sensor-form/register-device-window-sensor-form.component';
import { RegisterDeviceSmartLampFormComponent } from '../../../components/register-device-smart-lamp-form/register-device-smart-lamp-form.component';
import { DeviceRegisterSelectorService } from '../../../services/device-register-selector.service';


@Component({
  selector: 'app-company-owner-register-device-view',
  standalone: true,
  imports: [
    CommonModule,
    RegisterDeviceCameraFormComponent,
    RegisterDeviceMovementSensorComponent,
    RegisterDeviceWindowSensorFormComponent,
    RegisterDeviceSmartLampFormComponent
  ],
  templateUrl: './company-owner-register-device-view.component.html',
  styleUrl: './company-owner-register-device-view.component.css'
})
export class CompanyOwnerRegisterDeviceViewComponent implements OnInit {
  selectedDevice: string | null = null;

  constructor(public deviceSelectorService: DeviceRegisterSelectorService ){ }
  
  onDeviceChange(event: Event) {
    const selectedValue = (event.target as HTMLSelectElement).value;
    this.deviceSelectorService.setSelectedDevice(selectedValue);
  }
  
  ngOnInit(){
    this.deviceSelectorService.selectedDevice$.subscribe(device => {
      this.selectedDevice = device;
    })
  }
}
