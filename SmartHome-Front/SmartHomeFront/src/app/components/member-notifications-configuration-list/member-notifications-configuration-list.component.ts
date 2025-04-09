import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MemberNotificationsConfigurationListItemComponent } from '../member-notifications-configuration-list-item/member-notifications-configuration-list-item.component';
import { HomeMember } from '../../models/HomeMember';
import { HomeService } from '../../services/home.service';
import { Router } from '@angular/router';
import { ListHome } from '../../models/ListHome';


@Component({
  selector: 'app-member-notifications-configuration-list',
  standalone: true,
  imports: [
    CommonModule,
    MemberNotificationsConfigurationListItemComponent
  ],
  templateUrl: './member-notifications-configuration-list.component.html',
  styleUrl: './member-notifications-configuration-list.component.css'
})
export class MemberNotificationsConfigurationListComponent {
  constructor(
    private homeService: HomeService,
    private router: Router
  ) {}

  homes: ListHome[] = [];
  selectedHome: ListHome | null = null;
  homeMembers: HomeMember[] = [];

  ngOnInit(): void {
    this.homeService.getHomesIOwn().subscribe(homes => {
      this.homes = homes;
    });
  }

  onSelectHome(home: ListHome): void {
    this.selectedHome = home;
    this.homeService.getHomeMembers(home.homeId).subscribe(members => {
      this.homeMembers = members;
    });
  }

  onDeselectHome(): void {
    this.selectedHome = null;
    this.homeMembers = [];
  }

  onMemberNotificationsConfigurationChange(): void {
    
  }
}
