import { Component, Input } from '@angular/core';
import { UserAccountResponse } from '../../models/UserAccountResponse';

@Component({
  selector: 'app-user-account-item',
  standalone: true,
  imports: [],
  templateUrl: './user-account-list-item.component.html',
  styleUrl: './user-account-list-item.component.css'
})
export class UserAccountListItemComponent {
  @Input() userAccount!:UserAccountResponse;
  constructor() {}
}
