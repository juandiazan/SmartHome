import { Component } from '@angular/core';
import { FormItemComponent } from '../form-item/form-item.component'; 
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { HomeOwnerService } from '../../services/home-owner.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-homeowner-form',
  standalone: true,
  imports: [
    FormItemComponent, 
    CommonModule,
    ReactiveFormsModule 
  ],
  templateUrl: './register-homeowner-form.component.html',
  styleUrl: './register-homeowner-form.component.css'
})

export class RegisterHomeownerFormComponent {

  registerForm = new FormGroup({
    name: new FormControl('', Validators.required),
    surname: new FormControl('', Validators.required),
    profilePicture: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required)
  });

  errorMessage: string | null = null;
  successMessage: string | null = null;

  constructor(
    private homeOwnerService: HomeOwnerService,
    private router: Router
  ) {}

  register() {
    if (this.registerForm.invalid) {
      this.errorMessage = "All fields are required and must be valid";
      return;
    }

    const formData = {
      name: this.registerForm.value.name ?? '',
      surname: this.registerForm.value.surname ?? '',
      profilePicture: this.registerForm.value.profilePicture ?? '',
      email: this.registerForm.value.email ?? '',
      password: this.registerForm.value.password ?? '',
    };

    this.homeOwnerService.createHomeOwnerAccount(formData).subscribe({
      next: account => {
        this.successMessage = "Home owner created successfully";
        this.errorMessage = null;
        setTimeout(() => {
          this.successMessage = null;
        }, 5000);
      },
      error: error => {
        this.errorMessage = error.message || "Error creating home owner";
        this.successMessage = null;
        setTimeout(() => {
          this.errorMessage = null;
        }, 5000);
      }
    });
  }
}
