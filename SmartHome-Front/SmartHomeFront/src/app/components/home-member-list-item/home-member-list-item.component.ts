import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeMember } from '../../models/HomeMember';

@Component({
  selector: 'app-home-member-list-item',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './home-member-list-item.component.html',
  styleUrls: ['./home-member-list-item.component.css']
})
export class HomeMemberListItemComponent {
  @Input() member!: HomeMember;
}
