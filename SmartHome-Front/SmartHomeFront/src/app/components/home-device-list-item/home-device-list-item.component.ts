import { Component, Input } from '@angular/core';
import { ListDevice } from '../../models/ListDevice';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-home-device-list-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './home-device-list-item.component.html',
  styleUrl: './home-device-list-item.component.css'
})
export class HomeDeviceListItemComponent {
  constructor () { }
  
  @Input() ldevice!:ListDevice;
}
