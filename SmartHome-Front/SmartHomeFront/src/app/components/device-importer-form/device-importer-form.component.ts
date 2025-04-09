import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component';
import { FormControl, FormGroup,ReactiveFormsModule } from '@angular/forms';
import { CompanyService } from '../../services/company-service.service';
import { CommonModule } from '@angular/common';
import { ImportDeviceRequest } from '../../models/ImportDeviceRequest';
import { CompanyDeviceImporterSelectorComponent } from '../company-device-importer-selector/company-device-importer-selector.component';


@Component({
  selector: 'app-device-importer-form',
  standalone: true,
  imports: [
    FormItemComponent,
    CompanyDeviceImporterSelectorComponent,
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './device-importer-form.component.html',
  styleUrl: './device-importer-form.component.css'
})


export class DeviceImporterFormComponent {
  importedDevice : ImportDeviceRequest | null = null;
  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(private companyService: CompanyService) {}

  selectedImporter: string = '';

  onSelectionChange(value: string): void {
    this.selectedImporter = value; 
  }

  registerForm = new FormGroup({
    path: new FormControl(''),
    importer: new FormControl('')
  });

  clearMessages() {
    setTimeout(() => {
      this.errorMessage = null;
      this.successMessage = null;
    }, 5000);
  }

  importDevices() {
    const formData = {
      path: this.registerForm.value.path ?? '',
      importer: this.selectedImporter.toString() ?? ''
    };

    this.importedDevice = { deviceImporterId: formData.importer, filePath: formData.path };

    this.companyService.importDevices(this.importedDevice).subscribe({
      next: company => {
        this.successMessage = "Devices imported successfully";
        this.clearMessages();
      },
      error: error => {
        this.errorMessage = "Error importing devices";
        this.clearMessages();
      }
    });
  }
}
