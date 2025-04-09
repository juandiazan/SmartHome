import { Component, Input, OnInit } from '@angular/core';
import { HomeMember } from '../../models/HomeMember';
import { CommonModule } from '@angular/common';
import { HomeService } from '../../services/home.service';

@Component({
  selector: 'app-member-notifications-configuration-list-item',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './member-notifications-configuration-list-item.component.html',
  styleUrl: './member-notifications-configuration-list-item.component.css'
})
export class MemberNotificationsConfigurationListItemComponent implements OnInit {
  @Input() member!: HomeMember;
  @Input() homeId!: string;
  notificationsEnabled: boolean = false;

  constructor(private homeService: HomeService) {}

  ngOnInit(): void {
    this.notificationsEnabled = this.member.canReceiveNotifications;
  }

  onNotificationChange(event: any): void {
    this.notificationsEnabled = event.target.checked;
    this.homeService.updateMemberNotificationPermissions(this.notificationsEnabled, this.homeId, this.member.id).subscribe();
  }
}
