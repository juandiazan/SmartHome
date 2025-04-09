import { Component, OnInit } from '@angular/core';
import { HomeService } from '../../services/home.service';
import { HomeDeviceListItemComponent } from "../home-device-list-item/home-device-list-item.component";
import { CommonModule } from '@angular/common';
import { ListDevice } from '../../models/ListDevice';
import { ListHome } from '../../models/ListHome';
import { GenericFilterComponent } from '../generic-filter/generic-filter.component';

@Component({
  selector: 'app-home-device-list',
  standalone: true,
  imports: [HomeDeviceListItemComponent, CommonModule, GenericFilterComponent],
  templateUrl: './home-device-list.component.html',
  styleUrls: ['./home-device-list.component.css'],
})
export class HomeDeviceListComponent implements OnInit {
  currentHomeId: string = '';
  homes: ListHome[] = [];
  selectedHomeDevices: ListDevice[] = [];

  roomItIsIn: string = '';
  noHomes: boolean = false;
  noDevices: boolean = false;

  constructor(private homeService: HomeService) { }

  setRoom(room: string): void {
    this.roomItIsIn = room;
    this.viewDevices(this.currentHomeId, this.roomItIsIn);
  }

  ngOnInit(): void {
    this.homeService.getHomesWithListDevicesPermission().subscribe(homes => {
      this.homes = homes;
      this.noHomes = homes.length === 0;
    });
  }

  viewDevices(homeId: string, room: string): void {
    this.currentHomeId = homeId;
    this.homeService.getHomeDevices(homeId, room).subscribe(devices => {
      this.selectedHomeDevices = devices;
      this.noDevices = devices.length === 0;
    });
  }
}