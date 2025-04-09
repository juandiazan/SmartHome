import { Component, Input } from '@angular/core';
import { UserAccountResponse } from '../../models/UserAccountResponse';
import { UserDeleteService } from '../../services/user-delete.service';

@Component({
  selector: 'app-delete-user-account-list-item',
  standalone: true,
  imports: [],
  templateUrl: './delete-user-account-list-item.component.html',
  styleUrl: './delete-user-account-list-item.component.css'
})
export class DeleteUserAccountListItemComponent {
  @Input() userAccount!:UserAccountResponse;
  constructor(private userDeleteService: UserDeleteService) { }

  ngOnInit(){
    this.userAccount = this.userDeleteService.getUserToBeDeleted()!; 
  }

  onClick(){
    this.userDeleteService.deleteUser(this.userAccount);
  }
}
