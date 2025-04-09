import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component';
import { CompanyOwnerService } from '../../services/company-owner.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-device-camera-form',
  standalone: true,
  imports: [
    FormItemComponent, CommonModule, ReactiveFormsModule
  ],
  templateUrl: './register-device-camera-form.component.html',
  styleUrl: './register-device-camera-form.component.css'
})
export class RegisterDeviceCameraFormComponent {

  constructor(
    private companyOwnerService: CompanyOwnerService,
    private router: Router
  ) {}

  errorMessage: string | null = null;
  successMessage: string | null = null;

  registerCameraForm = new FormGroup({
    cameraName: new FormControl('', Validators.required),
    cameraModel: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    photos: new FormControl('', Validators.required),
    deviceType: new FormControl(false, Validators.required),
    canBeUsedIndoors: new FormControl(false, Validators.required),
    canBeUsedOutdoors: new FormControl(false, Validators.required),
    hasMovementDetectionSupport: new FormControl(false, Validators.required),
    hasPersonDetectionSupport: new FormControl(false, Validators.required),
  });

  private clearMessages() {
    setTimeout(() => {
      this.successMessage = null;
      this.errorMessage = null;
    }, 5000);
  }

  registerCamera() {
    if (this.registerCameraForm.invalid) {
      this.errorMessage = "All fields are required and must be valid";
      return;
    }

    const formData = {
      cameraName: this.registerCameraForm.value.cameraName ?? '',
      cameraModel: this.registerCameraForm.value.cameraModel ?? '',
      description: this.registerCameraForm.value.description ?? '',
      photos: Array.isArray(this.registerCameraForm.value.photos) ? this.registerCameraForm.value.photos : [this.registerCameraForm.value.photos ?? ""],
      deviceType: "Camera",
      canBeUsedIndoors: this.registerCameraForm.value.canBeUsedIndoors ?? false,
      canBeUsedOutdoors: this.registerCameraForm.value.canBeUsedOutdoors ?? false,
      hasMovementDetectionSupport: this.registerCameraForm.value.hasMovementDetectionSupport ?? false,
      hasPersonDetectionSupport: this.registerCameraForm.value.hasPersonDetectionSupport ?? false,
    };

    this.companyOwnerService.registerCamera(formData).subscribe({
      next: camera => {
        this.successMessage = "Device created successfully";
        this.errorMessage = null;
        this.clearMessages();
      },
      error: error => {
        this.errorMessage = error.message || "Error creating device";
        this.successMessage = null;
        this.clearMessages();
      }
    });
  }
}