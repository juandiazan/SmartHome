import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ListHome } from '../../models/ListHome';
import { HomeService } from '../../services/home.service';
import { HomeDevice } from '../../models/HomeDevice';
import { HomeDeviceService } from '../../services/home-device.service';

@Component({
  selector: 'app-custom-device-name',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './custom-device-name.component.html',
  styleUrl: './custom-device-name.component.css'
})
export class CustomDeviceNameComponent implements OnInit {

  homes: ListHome[] = [];
  devices: HomeDevice[] = [];
  modifyDeviceForm: FormGroup;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private homeService: HomeService,
    private homeDeviceService: HomeDeviceService,
    private fb: FormBuilder,
  ) {
    this.modifyDeviceForm = this.fb.group({
      homeId: [{ value: '', disabled: true }, Validators.required],
      deviceId: [{ value: '', disabled: true }, Validators.required],
      alias: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.homeService.getHomesWithModifyDeviceNamePermission().subscribe(homes => {
      this.homes = homes;
      if (this.homes.length > 0) {
        this.modifyDeviceForm.get('homeId')?.enable();
      }
    });

    this.modifyDeviceForm.get('homeId')?.valueChanges.subscribe(homeId => {
      if (homeId) {
        this.homeService.getHomeDevices(homeId, '').subscribe(devices => {
          this.devices = devices;
          if (this.devices.length > 0) {
            this.modifyDeviceForm.get('deviceId')?.enable();
          } else {
            this.modifyDeviceForm.get('deviceId')?.disable();
          }
        });
      }
    });
  }

  onSubmit(): void {
    if (this.modifyDeviceForm.valid && this.homes.length > 0 && this.devices.length > 0) {
      const { deviceId, alias } = this.modifyDeviceForm.value;
      this.homeDeviceService.modifyHomeDeviceAlias(deviceId, alias).subscribe({
        next: () => {
          this.successMessage = 'Device name modified successfully!';
          this.errorMessage = null;
          this.clearMessagesAfterDelay();
        },
        error: error => {
          this.successMessage = null;
          this.errorMessage = error?.message || 'Error modifying device name';
          this.clearMessagesAfterDelay();
        }
      });
    } else {
      this.errorMessage = 'All fields are required.';
      this.clearMessagesAfterDelay();
    }
  }

  private clearMessagesAfterDelay(): void {
    setTimeout(() => {
      this.successMessage = null;
      this.errorMessage = null;
    }, 5000);
  }
}