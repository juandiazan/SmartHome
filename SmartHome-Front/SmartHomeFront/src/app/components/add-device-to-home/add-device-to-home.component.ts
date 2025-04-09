import { Component, OnInit } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HomeService } from '../../services/home.service';
import { DeviceService } from '../../services/device.service';
import { ListHome } from '../../models/ListHome';
import { Device } from '../../models/Device';

@Component({
  selector: 'app-add-device-to-home',
  standalone: true,
  imports: [
    CommonModule, ReactiveFormsModule
  ],
  templateUrl: './add-device-to-home.component.html',
  styleUrl: './add-device-to-home.component.css'
})
export class AddDeviceToHomeComponent implements OnInit {
  homes: ListHome[] = [];
  devices: Device[] = [];
  addDeviceForm: FormGroup;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private homeService: HomeService,
    private deviceService: DeviceService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.addDeviceForm = this.fb.group({
      homeId: [{ value: '', disabled: true }, Validators.required],
      deviceId: [{ value: '', disabled: true }, Validators.required],
      alias: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.homeService.getHomesWithAddDevicePermission().subscribe(homes => {
      this.homes = homes;
      if (this.homes.length > 0) {
        this.addDeviceForm.get('homeId')?.enable();
      }
    });

    this.deviceService.getAllDevices().subscribe(devices => {
      this.devices = devices;
      if (this.devices.length > 0) {
        this.addDeviceForm.get('deviceId')?.enable();
      }
    });
  }

  onSubmit(): void {
    if (this.addDeviceForm.valid && this.homes.length > 0 && this.devices.length > 0) {
      const { homeId, deviceId, alias } = this.addDeviceForm.value;
      this.homeService.addDeviceToHome(homeId, alias, deviceId).subscribe({
        next: () => {
          this.successMessage = 'Device added successfully!';
          this.errorMessage = null;
          setTimeout(() => this.successMessage = null, 5000);
        },
        error: (err) => {
          this.errorMessage = 'Failed to add device. Please try again.';
          this.successMessage = null;
          setTimeout(() => this.errorMessage = null, 5000);
        }
      });
    } else {
      this.errorMessage = 'Please select a home and a device.';
      this.successMessage = null;
    }
  }
}