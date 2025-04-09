import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component';
import { CompanyOwnerService } from '../../services/company-owner.service';
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-device-smart-lamp-form',
  standalone: true,
  imports: [
    FormItemComponent, ReactiveFormsModule, CommonModule
  ],
  templateUrl: './register-device-smart-lamp-form.component.html',
  styleUrl: './register-device-smart-lamp-form.component.css'
})
export class RegisterDeviceSmartLampFormComponent {

  constructor(
    private companyOwnerService: CompanyOwnerService,
    private router: Router
  ) { }

  errorMessage: string | null = null;
  successMessage: string | null = null;

  registerDeviceForm = new FormGroup({
    lampName: new FormControl('', Validators.required),
    lampModel: new FormControl('', Validators.required),
    description: new FormControl('', Validators.required),
    photos: new FormControl('', Validators.required),
    isTurnedOn: new FormControl(false, Validators.required)
  });

  registerDevice() {
    if (this.registerDeviceForm.invalid) {
      this.errorMessage = "All fields are required and must be valid";
      this.clearMessagesAfterDelay();
      return;
    }

    const formData = {
      lampName: this.registerDeviceForm.value.lampName ?? '',
      lampModel: this.registerDeviceForm.value.lampModel ?? '',
      description: this.registerDeviceForm.value.description ?? '',
      photos: Array.isArray(this.registerDeviceForm.value.photos) ? this.registerDeviceForm.value.photos : [this.registerDeviceForm.value.photos ?? ""],
      deviceType: "SmartLamp",
      isTurnedOn: this.registerDeviceForm.value.isTurnedOn ?? false,
    };

    this.companyOwnerService.registerSmartLamp(formData).subscribe({
      next: lamp => {
        this.successMessage = "Device created successfully";
        this.clearMessagesAfterDelay();
      },
      error: error => {
        this.errorMessage = error.message || "Error creating device";
        this.clearMessagesAfterDelay();
      }
    });
  }

  private clearMessagesAfterDelay() {
    setTimeout(() => {
      this.errorMessage = null;
      this.successMessage = null;
    }, 5000);
  }
}

