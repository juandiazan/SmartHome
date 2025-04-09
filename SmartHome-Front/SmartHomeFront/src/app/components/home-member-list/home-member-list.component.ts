import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeMemberListItemComponent } from '../home-member-list-item/home-member-list-item.component';
import { HomeMember } from '../../models/HomeMember';
import { Router } from '@angular/router';
import { HomeService } from '../../services/home.service';
import { ListHome } from '../../models/ListHome';

@Component({
  selector: 'app-home-member-list',
  standalone: true,
  imports: [
    CommonModule,
    HomeMemberListItemComponent
  ],
  templateUrl: './home-member-list.component.html',
  styleUrl: './home-member-list.component.css'
})
export class HomeMemberListComponent {
  
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
}
