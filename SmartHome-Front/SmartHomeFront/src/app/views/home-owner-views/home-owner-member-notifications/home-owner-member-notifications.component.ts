import { Component } from '@angular/core';
import { MemberNotificationsConfigurationListComponent } from '../../../components/member-notifications-configuration-list/member-notifications-configuration-list.component';

@Component({
  selector: 'app-home-owner-member-notifications',
  standalone: true,
  imports: [
    MemberNotificationsConfigurationListComponent
  ],
  templateUrl: './home-owner-member-notifications.component.html',
  styleUrl: './home-owner-member-notifications.component.css'
})
export class HomeOwnerMemberNotificationsComponent {

}
