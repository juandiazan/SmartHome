import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { UserService } from '../../services/user-service.service';
import { AuthService } from '../../services/auth.service';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register-as-home-owner',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './register-as-home-owner.component.html',
  styleUrl: './register-as-home-owner.component.css'
})
export class RegisterAsHomeOwnerComponent {
  message: string | null = null;

  constructor(private userService: UserService, private authService : AuthService, private router: Router) { }

  private clearMessages() {
    setTimeout(() => {
      this.message = null;
      this.router.navigate(['/home']);
    }, 4000);
  }

  onClick(): void {
    const role = this.authService.getRole();

    if(role === 'company-owner') {
      this.userService.giveHomeOwnerRoleToCompanyOwner().subscribe({
        next: (response) => {
          this.message = response;
          this.clearMessages();
        },
        error: (error) => {
          this.message = "error: " + error.message;
          this.clearMessages();
        }
      });
      return;
    }

    this.userService.giveHomeOwnerRoleToAdmin().subscribe({
      next: (response) => {
        this.message = response;
        this.clearMessages();
      },
      error: (error) => {
        this.message = "error: " + error.message;
        this.clearMessages();
      }
    });
  }
}
