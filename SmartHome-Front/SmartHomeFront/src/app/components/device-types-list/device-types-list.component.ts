import { Component, OnInit } from '@angular/core';
import  { CommonModule } from '@angular/common';
import { DeviceTypeItemComponent } from '../device-type-item/device-type-item.component';
import { DeviceTypesService } from '../../services/device-types.service';


@Component({
  selector: 'app-device-types-list',
  standalone: true,
  imports: [
    CommonModule,
  ],
  templateUrl: './device-types-list.component.html',
  styleUrl: './device-types-list.component.css'
})
export class DeviceTypesListComponent {
  devices:string[] = [];

  constructor(private deviceTypesServices: DeviceTypesService) {}

  ngOnInit():void {
    this.deviceTypesServices.getAllDeviceTypes().subscribe(
      (data) => {
        this.devices = data;
      },
      (error) => {
        console.error('Error fetching device types', error);
      }
    );
  }
}
