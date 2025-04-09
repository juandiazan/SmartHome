import { Component } from '@angular/core';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { HomeService } from '../../services/home.service';
import { ListHome } from '../../models/ListHome';

@Component({
  selector: 'app-add-room-to-home',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './add-room-to-home.component.html',
  styleUrl: './add-room-to-home.component.css'
})
export class AddRoomToHomeComponent {
  homes: ListHome[] = [];
  addRoomForm: FormGroup;
  successMessage: string | null = null;
  errorMessage: string | null = null;

  constructor(
    private homeService: HomeService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.addRoomForm = this.fb.group({
      homeId: [{ value: '', disabled: true }, Validators.required],
      roomName: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.homeService.getHomes().subscribe(homes => {
      this.homes = homes;
      if (this.homes.length > 0) {
        this.addRoomForm.get('homeId')?.enable();
      }
    });
  }

  onSubmit(): void {
    if (this.addRoomForm.valid && this.homes.length > 0) {
      const { homeId, roomName } = this.addRoomForm.value;
      this.homeService.addRoomToHome(homeId, roomName).subscribe({
        next: () => {
          this.successMessage = 'Room created successfully!';
          this.errorMessage = null;
          this.clearMessagesAfterTimeout();
        },
        error: (err) => {
          this.errorMessage = 'Failed to create room. Please try again.';
          this.successMessage = null;
          this.clearMessagesAfterTimeout();
        }
      });
    } else {
      this.errorMessage = 'Please select a home.';
      this.successMessage = null;
      this.clearMessagesAfterTimeout();
    }
  }

  private clearMessagesAfterTimeout(): void {
    setTimeout(() => {
      this.successMessage = null;
      this.errorMessage = null;
    }, 5000);
  }
}
