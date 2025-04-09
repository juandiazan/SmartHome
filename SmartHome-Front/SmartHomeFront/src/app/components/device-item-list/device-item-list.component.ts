import { Component, Input } from '@angular/core';
import { Device } from "../../models/Device";

@Component({
  selector: 'app-device-item-list',
  standalone: true,
  imports: [],
  templateUrl: './device-item-list.component.html',
  styleUrl: './device-item-list.component.css'
})

export class DeviceItemListComponent {
  @Input() device!:Device;
  
  constructor () {}
}
