import { Component } from '@angular/core';
import { HomeDeviceListComponent } from '../../../components/home-device-list/home-device-list.component';

@Component({
  selector: 'app-home-owner-see-home-devices',
  standalone: true,
  imports: [HomeDeviceListComponent],
  templateUrl: './home-owner-see-home-devices.component.html',
  styleUrl: './home-owner-see-home-devices.component.css'
})
export class HomeOwnerSeeHomeDevicesComponent {

}
