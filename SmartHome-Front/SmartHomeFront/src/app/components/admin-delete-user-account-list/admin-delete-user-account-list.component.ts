import { Component } from '@angular/core';
import { DeleteUserAccountListItemComponent } from '../delete-user-account-list-item/delete-user-account-list-item.component';
import { CommonModule } from '@angular/common';
import { UserService } from '../../services/user-service.service';
import { UserAccountResponse } from '../../models/UserAccountResponse';
import { RouterModule } from '@angular/router';
import { UserAccountListItemComponent } from "../user-account-list-item/user-account-list-item.component";
import { Router } from '@angular/router';
import { UserDeleteService } from '../../services/user-delete.service';

@Component({
  selector: 'app-admin-delete-user-account-list',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    UserAccountListItemComponent
],
  templateUrl: './admin-delete-user-account-list.component.html',
  styleUrl: './admin-delete-user-account-list.component.css'
})
export class AdminDeleteUserAccountListComponent {
  offset : number = 1;
  limit : number = 10;
  
  apiAccounts: UserAccountResponse[] = [];
  
  constructor(private userService: UserService, private router: Router, private userDeleteService: UserDeleteService) { }
  
  ngOnInit(){
    this.loadAccounts();
  }

  loadAccounts(){
    this.userService.getAccounts(this.offset, this.limit, "", "").subscribe(({
      next: (data) => this.apiAccounts = data,
      error: (error) => console.error(error)
    }));
  }

  onClick(user: UserAccountResponse){
    this.userDeleteService.setUserToBeDeleted(user);
    this.router.navigate(["/delete-users", user.id]);
  }
}
