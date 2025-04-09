import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserAccountListItemComponent } from "../user-account-list-item/user-account-list-item.component"
import { UserService } from "../../services/user-service.service"
import { UserAccountResponse } from "../../models/UserAccountResponse"
import { PaginationComponent } from "../pagination/pagination.component";
import { GenericFilterComponent } from '../generic-filter/generic-filter.component';

@Component({
  selector: 'app-user-accounts-list',
  standalone: true,
  imports: [
    CommonModule,
    UserAccountListItemComponent,
    PaginationComponent,
    GenericFilterComponent
],
  templateUrl: './user-accounts-list.component.html',
  styleUrl: './user-accounts-list.component.css'
})

export class UserAccountsListComponent {
  offset : number = 1;
  limit : number= 10;
  role : string = "";
  fullName : string = "";
  
  setOffset(newOffset: number) {
    this.offset = newOffset;
    this.loadAccounts();
  }
  
  setLimit(newLimit: number) {
    this.limit = newLimit;
    this.loadAccounts();
  }

  setRole(newRole: string) {
    this.role = newRole;
    this.loadAccounts();
  }

  setName(newName: string) {
    this.fullName = newName;
    this.loadAccounts();
  }
  
  apiAccounts: UserAccountResponse[] = [];
  
  constructor(private userService: UserService) { }
  
  ngOnInit(){
    this.loadAccounts();
  }

  loadAccounts(){
    this.userService.getAccounts(this.offset, this.limit, this.role, this.fullName).subscribe(({
      next: (data) => this.apiAccounts = data,
      error: (error) => console.error(error)
    }));
  }
}
