import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component';
import { CompanyOwnerService } from '../../services/company-owner.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-device-movement-sensor',
  standalone: true,
  imports: [
    FormItemComponent, ReactiveFormsModule, CommonModule
  ],
  templateUrl: './register-device-movement-sensor.component.html',
  styleUrl: './register-device-movement-sensor.component.css'
})
export class RegisterDeviceMovementSensorComponent {

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
      return;
    }

    const formData = {
      deviceName: this.registerDeviceForm.value.deviceName ?? '',
      deviceModel: this.registerDeviceForm.value.deviceModel ?? '',
      description: this.registerDeviceForm.value.description ?? '',
      photos: Array.isArray(this.registerDeviceForm.value.photos) ? this.registerDeviceForm.value.photos : [this.registerDeviceForm.value.photos ?? ""],
      deviceType: "MovementSensor"
    };

    this.companyOwnerService.registerDevice(formData).subscribe({
      next: device => {
        this.successMessage = "Device created successfully";
        this.clearMessages();
      },
      error: error => {
        this.errorMessage = error.message || "Error creating device";
        this.clearMessages();
      }
    });
  }

  private clearMessages() {
    setTimeout(() => {
      this.errorMessage = null;
      this.successMessage = null;
    }, 5000);
  }
}