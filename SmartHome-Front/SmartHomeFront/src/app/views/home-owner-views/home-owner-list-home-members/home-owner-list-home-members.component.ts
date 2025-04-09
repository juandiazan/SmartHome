import { Component } from '@angular/core';
import { HomeMemberListComponent } from '../../../components/home-member-list/home-member-list.component';

@Component({
  selector: 'app-home-owner-list-home-members',
  standalone: true,
  imports: [
    HomeMemberListComponent
  ],
  templateUrl: './home-owner-list-home-members.component.html',
  styleUrl: './home-owner-list-home-members.component.css'
})
export class HomeOwnerListHomeMembersComponent {
  constructor () {}
}
