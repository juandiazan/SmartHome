import { Component } from '@angular/core';
import { AddDeviceToHomeComponent } from '../../../components/add-device-to-home/add-device-to-home.component';


@Component({
  selector: 'app-home-owner-add-device-to-home',
  standalone: true,
  imports: [
    AddDeviceToHomeComponent
  ],
  templateUrl: './home-owner-add-device-to-home.component.html',
  styleUrl: './home-owner-add-device-to-home.component.css'
})
export class HomeOwnerAddDeviceToHomeComponent {

}
