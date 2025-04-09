import { Component } from '@angular/core';
import { RegisterUserComponent } from '../../../components/register-user/register-user.component';
import { AdminUserTypeSelectorComponent } from '../../../components/admin-user-type-selector/admin-user-type-selector.component';

@Component({
  selector: 'app-admin-register-user-form',
  standalone: true,
  imports: [ 
    RegisterUserComponent, 
    AdminUserTypeSelectorComponent
  ],
  templateUrl: './admin-register-user-form.component.html',
  styleUrl: './admin-register-user-form.component.css'
})
export class AdminRegisterUserFormComponent {
  constructor (){ }
}
