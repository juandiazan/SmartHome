import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CompanyOwnerService } from '../../services/company-owner.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-device-window-sensor-form',
  standalone: true,
  imports: [
    FormItemComponent, ReactiveFormsModule, CommonModule
  ],
  templateUrl: './register-device-window-sensor-form.component.html',
  styleUrl: './register-device-window-sensor-form.component.css'
})


export class RegisterDeviceWindowSensorFormComponent {

  constructor(
    private companyOwnerService: CompanyOwnerService,
    private router: Router
  ) {}

  errorMessage: string | null = null;
  successMessage: string | null = null;

  registerDeviceForm = new FormGroup({
    deviceName: new FormControl('', Validators.required),
    deviceModel: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    photos: new FormControl('', Validators.required),
  });

  registerDevice() {
    if (this.registerDeviceForm.invalid) {
      this.errorMessage = "All fields are required and must be valid";
      setTimeout(() => {
        this.errorMessage = null;
      }, 5000);
      return;
    }

    const formData = {
      deviceName: this.registerDeviceForm.value.deviceName ?? '',
      deviceModel: this.registerDeviceForm.value.deviceModel ?? '',
      description: this.registerDeviceForm.value.description ?? '',
      photos: Array.isArray(this.registerDeviceForm.value.photos) ? this.registerDeviceForm.value.photos : [this.registerDeviceForm.value.photos ?? ""],
      deviceType: "Sensor"
    };

    this.companyOwnerService.registerDevice(formData).subscribe({
      next: device => {
        this.successMessage = "Device created successfully";
        this.errorMessage = null;
        setTimeout(() => {
          this.successMessage = null;
        }, 5000);
      },
      error: error => {
        this.errorMessage = error.message || "Error creating device";
        this.successMessage = null;
        setTimeout(() => {
          this.errorMessage = null;
        }, 5000);
      }
    });
  }
}

