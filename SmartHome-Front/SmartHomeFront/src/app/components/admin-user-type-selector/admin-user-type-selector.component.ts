import { Component } from '@angular/core';
import { UserTypeService } from '../../services/user-type-service.service';

@Component({
  selector: 'app-admin-user-type-selector',
  standalone: true,
  imports: [],
  templateUrl: './admin-user-type-selector.component.html',
  styleUrl: './admin-user-type-selector.component.css'
})
export class AdminUserTypeSelectorComponent {
  constructor(private userTypeService: UserTypeService) {}

  onUserTypeChange(event: Event) {
    const target = event.target as HTMLSelectElement;
    if (target && target.value) {
      this.userTypeService.changeUserType(target.value);
    }
  }
}