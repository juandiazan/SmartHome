import { Component } from '@angular/core';
import { DeleteUserAccountListItemComponent } from '../../../components/delete-user-account-list-item/delete-user-account-list-item.component';
import { AdminDeleteUserAccountListComponent } from '../../../components/admin-delete-user-account-list/admin-delete-user-account-list.component';

@Component({
  selector: 'app-admin-delete-admins-accounts',
  standalone: true,
  imports: [
    DeleteUserAccountListItemComponent,
    AdminDeleteUserAccountListComponent
  ],
  templateUrl: './admin-delete-admins-accounts.component.html',
  styleUrl: './admin-delete-admins-accounts.component.css'
})
export class AdminDeleteAdminsAccountsComponent {

}
