import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component';
import { CommonModule } from '@angular/common';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HomeService } from '../../services/home.service';

@Component({
  selector: 'app-create-home-form',
  standalone: true,
  imports: [
    FormItemComponent, CommonModule, ReactiveFormsModule
  ],
  templateUrl: './create-home-form.component.html',
  styleUrl: './create-home-form.component.css'
})

export class CreateHomeFormComponent {

  createHomeForm = new FormGroup({
    mainStreet: new FormControl('', Validators.required),
    doorNumber: new FormControl('', [Validators.required, Validators.pattern('^[0-9]*$')]),
    latitude: new FormControl('', Validators.required),
    longitude: new FormControl('', Validators.required),
    maxAmountOfMembers: new FormControl('', [Validators.required, Validators.pattern('^[0-9]*$')]),
    alias: new FormControl('', Validators.required)
  });

  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(
    private homeService: HomeService,
    private router: Router
  ) {}

  clearMessages() {
    setTimeout(() => {
      this.errorMessage = null;
      this.successMessage = null;
    }, 5000);
  }

  createHome() {
    if (this.createHomeForm.invalid) {
      this.errorMessage = "All fields are required and must be valid";
      this.successMessage = null;
      this.clearMessages();
      return;
    }

    const formData = {
      ownerEmail: '',
      mainStreet: this.createHomeForm.value.mainStreet ?? '',
      doorNumber: Number(this.createHomeForm.value.doorNumber) ?? 0,
      latitude: this.createHomeForm.value.latitude ?? '',
      longitude: this.createHomeForm.value.longitude ?? '',
      maxAmountOfMembers: Number(this.createHomeForm.value.maxAmountOfMembers) ?? 0,
      alias: this.createHomeForm.value.alias ?? '',
    };

    this.homeService.createHome(formData).subscribe({
      next: home => {
        this.successMessage = "Home created successfully";
        this.errorMessage = null;
        this.clearMessages();
      },
      error: error => {
        this.errorMessage = error?.message || "Error creating home";
        this.successMessage = null;
        this.clearMessages();
      }
    });
  }
}
