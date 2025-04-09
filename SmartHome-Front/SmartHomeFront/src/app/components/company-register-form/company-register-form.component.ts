import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component';
import { CompanyModelValidatorSelectorComponent } from '../company-model-validator-selector/company-model-validator-selector.component';
import { FormControl, FormGroup,ReactiveFormsModule } from '@angular/forms';
import { CompanyCreateRequest } from '../../models/CompanyCreateRequest'; 
import { CompanyService } from '../../services/company-service.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-company-register-form',
  standalone: true,
  imports: [
    FormItemComponent,
    CompanyModelValidatorSelectorComponent,
    ReactiveFormsModule,
    CommonModule
  ],
  templateUrl: './company-register-form.component.html',
  styleUrl: './company-register-form.component.css'
})

export class CompanyRegisterFormComponent {
  createdCompany : CompanyCreateRequest | null = null;
  errorMessage: string | null = null;
  successMessage: string | null = null; 
  constructor(private companyService: CompanyService, private router: Router) {}

  selectedModel: string = '';
  onSelectionChange(value: string): void {
    this.selectedModel = value; 
  }

  registerForm = new FormGroup({
    name: new FormControl(''),
    logo: new FormControl(''),
    rut: new FormControl('')
  });

  clearMessages() {
    setTimeout(() => {
      this.successMessage = null;
      this.errorMessage = null;
    }, 5000);
  }

  registerCompany() {
    const formData = {
      name: this.registerForm.value.name ?? '',
      logo: this.registerForm.value.logo ?? '',
      rut: this.registerForm.value.rut ?? '',
    };

    this.createdCompany = { name: formData.name, logo: formData.logo, rut: formData.rut, modelValidator: this.selectedModel };

    this.companyService.createCompany(this.createdCompany).subscribe({
      next: company => {
        this.successMessage = "Company created successfully"; 
        this.errorMessage = null; 
        this.clearMessages(); 
      },
      error: error => {
        this.errorMessage = error.message || "Server error";
        this.successMessage = null; 
        this.clearMessages(); 
      }
    });
  }
}
