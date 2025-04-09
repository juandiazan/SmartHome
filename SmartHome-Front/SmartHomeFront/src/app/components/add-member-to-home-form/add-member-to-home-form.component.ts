import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormItemComponent } from '../form-item/form-item.component';
import { HomeService } from '../../services/home.service';
import { HomeOwnerService } from '../../services/home-owner.service';
import { ListHome } from '../../models/ListHome';

@Component({
  selector: 'app-add-member-to-home-form',
  standalone: true,
  imports: [
    FormItemComponent, CommonModule, ReactiveFormsModule
  ],
  templateUrl: './add-member-to-home-form.component.html',
  styleUrl: './add-member-to-home-form.component.css'
})
export class AddMemberToHomeFormComponent implements OnInit {
  homes: ListHome[] = [];
  successMessage: string | null = null;

  addMemberToHomeForm = new FormGroup({
    homeId: new FormControl('', Validators.required),
    emailOfNewMember: new FormControl('', [Validators.required, Validators.email]),
    canAddDeviceToHome: new FormControl(false),
    canSeeDevicesOfHome: new FormControl(false),
    canChangeAliasOfDevices: new FormControl(false)
  });

  errorMessage: string | null = null;

  constructor(
    private homeService: HomeService,
    private homeOwnerService: HomeOwnerService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.homeService.getHomesWithAddDevicePermission().subscribe(homes => {
      this.homes = homes;
    });
  }

  addMemberToHome() {
    if (this.addMemberToHomeForm.invalid) {
      this.errorMessage = "All fields are required and must be valid";
      return;
    }

    const homeId = this.addMemberToHomeForm.value.homeId ?? '';
    const formData = {
      emailOfNewMember: this.addMemberToHomeForm.value.emailOfNewMember ?? '',
      canAddDeviceToHome: this.addMemberToHomeForm.value.canAddDeviceToHome ?? false,
      canSeeDevicesOfHome: this.addMemberToHomeForm.value.canSeeDevicesOfHome ?? false,
      canChangeAliasOfDevices: this.addMemberToHomeForm.value.canChangeAliasOfDevices ?? false
    };

    this.homeService.addMemberToHome(homeId, formData).subscribe({
      next: account => {
        this.successMessage = "Member added to home successfully";
        setTimeout(() => this.successMessage = null, 5000);
      },
      error: error => {
        this.errorMessage = error.message || "Error adding member to home";
        setTimeout(() => this.errorMessage = null, 5000);
      }
    });
  }
}