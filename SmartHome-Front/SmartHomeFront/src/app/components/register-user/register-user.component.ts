import { Component } from '@angular/core';
import { FormItemComponent } from "../form-item/form-item.component";
import { Router } from '@angular/router';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserTypeService } from '../../services/user-type-service.service';
import { UserService } from '../../services/user-service.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-register-user',
  standalone: true,
  imports: [FormItemComponent, ReactiveFormsModule, CommonModule],
  templateUrl: './register-user.component.html',
  styleUrl: './register-user.component.css'
})
export class RegisterUserComponent {
  shownUserRole: string = 'Administrador';
  errorMessage: string | null = null;
  successMessage: string | null = null;

  registerForm = new FormGroup({
    name: new FormControl('', Validators.required),
    surname: new FormControl('', Validators.required),
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', Validators.required)
  });

  constructor(
    private userTypeService: UserTypeService, 
    private userService : UserService,
    private router : Router) {}

  ngOnInit(): void {
    this.userTypeService.currentUserType$.subscribe(type => {
      this.shownUserRole = type === "administrator" ? "Administrator" : "Company Owner"; 
    });
  }

  register() {   
    if (this.registerForm.invalid) {
      this.registerForm.markAllAsTouched();
      this.errorMessage = "Please fill out all fields correctly.";
      this.clearMessagesAfterDelay();
      return;
    }
    const formData = {
      name: this.registerForm.value.name ?? '',
      surname: this.registerForm.value.surname ?? '',
      email: this.registerForm.value.email ?? '',
      password: this.registerForm.value.password ?? '',
    };

    this.userService.createAccount(formData).subscribe({
      next: account => { 
        this.successMessage = "User created successfully";
        this.clearMessagesAfterDelay();
      },
      error: error => {
        this.errorMessage = error.error.message || "Error creating user";
        this.clearMessagesAfterDelay();
      }
    });
  }

  private clearMessagesAfterDelay() {
    setTimeout(() => {
      this.successMessage = null;
      this.errorMessage = null;
    }, 5000);
  }
}
