import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HomeService } from '../../services/home.service';
import { RoomService } from '../../services/room.service';
import { ListHome } from '../../models/ListHome';
import { ListDevice } from '../../models/ListDevice';
import { HomeRoom } from '../../models/HomeRoom';

@Component({
  selector: 'app-add-device-to-room',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule
  ],
  templateUrl: './add-device-to-room.component.html',
  styleUrl: './add-device-to-room.component.css'
})
export class AddDeviceToRoomComponent {
  homes: ListHome[] = [];
  homeDevices: ListDevice[] = [];
  rooms: HomeRoom[] = [];

  addDeviceToRoomForm: FormGroup;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private homeService: HomeService,
    private roomService: RoomService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.addDeviceToRoomForm = this.fb.group({
      homeId: [{ value: '', disabled: true }, Validators.required],
      roomId: [{ value: '', disabled: true }, Validators.required],
      hardwareId: [{ value: '', disabled: true }, Validators.required],
    });
  }

  ngOnInit(): void {
    this.homeService.getHomes().subscribe(homes => {
      this.homes = homes;
      if (this.homes.length > 0) {
        this.addDeviceToRoomForm.get('homeId')?.enable();
      }
    });

    this.addDeviceToRoomForm.get('homeId')?.valueChanges.subscribe(homeId => {
      if (homeId) {
        this.fetchRooms(homeId); 
      } else {
        this.rooms = []; 
        this.addDeviceToRoomForm.get('roomId')?.disable();
      }
    });

    this.addDeviceToRoomForm.get('homeId')?.valueChanges.subscribe(homeId => {
      if (homeId) {
        this.fetchHomeDevices(homeId); 
      } else {
        this.homeDevices = []; 
        this.addDeviceToRoomForm.get('hardwareId')?.disable();
      }
    });
  }

  private fetchRooms(homeId: string): void {
    this.roomService.getRoomsOfHome(homeId).subscribe({
      next: (rooms: HomeRoom[]) => {
        this.rooms = rooms;
        if (this.rooms.length > 0) {
          this.addDeviceToRoomForm.get('roomId')?.enable();
        } else {
          this.addDeviceToRoomForm.get('roomId')?.disable();
        }
      },
      error: () => {
        this.rooms = [];
        this.addDeviceToRoomForm.get('roomId')?.disable();
      }
    });
  }

  private fetchHomeDevices(homeId: string): void {
    this.homeService.getHomeDevices(homeId, '').subscribe({
      next: (devices: ListDevice[]) => {
        this.homeDevices = devices;
        if (this.homeDevices.length > 0) {
          this.addDeviceToRoomForm.get('hardwareId')?.enable();
        } else {
          this.addDeviceToRoomForm.get('hardwareId')?.disable();
        }
      },
      error: () => {
        this.homeDevices = [];
        this.addDeviceToRoomForm.get('hardwareId')?.disable();
      }
    });
  }

  onSubmit(): void {
    if (this.addDeviceToRoomForm.valid && this.homes.length > 0 && this.rooms.length > 0 && this.homeDevices.length > 0) {
      const { homeId, roomId, hardwareId } = this.addDeviceToRoomForm.value;
      this.roomService.addDeviceToRoom(roomId, hardwareId).subscribe({
        next: () => {
          this.successMessage = 'Device added successfully to room!';
          this.errorMessage = null;
          setTimeout(() => this.successMessage = null, 5000);
        },
        error: (err: any) => {
          this.errorMessage = 'Failed to add device to room. Please try again.';
          this.successMessage = null;
          setTimeout(() => this.errorMessage = null, 5000);
        }
      });
    } else {
      this.errorMessage = 'Please select a home, a room and a home device.';
      this.successMessage = null;
    }
  }
}
