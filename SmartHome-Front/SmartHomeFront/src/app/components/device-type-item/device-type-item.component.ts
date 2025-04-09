import { Component, Input} from '@angular/core';
import { DeviceType } from '../../models/DeviceType';


@Component({
  selector: 'app-device-type-item',
  standalone: true,
  imports: [],
  templateUrl: './device-type-item.component.html',
  styleUrl: './device-type-item.component.css'
})
export class DeviceTypeItemComponent {
  @Input() device!:DeviceType; 
}
