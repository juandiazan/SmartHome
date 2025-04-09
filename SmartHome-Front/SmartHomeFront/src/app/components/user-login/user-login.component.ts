import { Router, RouterModule } from '@angular/router';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormControl, FormGroup } from '@angular/forms';
import { FormItemComponent } from '../form-item/form-item.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-user-login',
  standalone: true,
  imports: [ 
    FormItemComponent,
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    RouterModule
   ],
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {
  loginForm = new FormGroup({
    email: new FormControl(''), 
    password: new FormControl('')
  });

  constructor (private authService: AuthService, private router : Router) { } 
  
  email: string = ''; 
  password: string = ''; 
  errorMessage: string | null = null;

  onLogin(): void {
    const formData = {
      email: this.loginForm.value.email ?? '',
      password: this.loginForm.value.password ?? ''
    };

    this.authService.login(formData).subscribe({
      next: (response) => {
        localStorage.setItem('token', response);
  
        this.authService.fetchUserRole().subscribe({
          next: (role) => {
            this.authService.setRole(role);
            this.router.navigate(['/home']);
          }
        });
      },
      error: (loginError) => {
        this.errorMessage = "Error logging in. Check your credentials.";
      }
    });
  }
  
}
