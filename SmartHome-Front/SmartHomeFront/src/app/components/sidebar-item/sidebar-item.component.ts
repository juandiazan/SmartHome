import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-sidebar-item',
  standalone: true,
  imports: [
    RouterModule,
    MatIconModule,
    CommonModule
  ],
  templateUrl: './sidebar-item.component.html',
  styleUrls: ['./sidebar-item.component.css']
})
export class SidebarItemComponent {
  constructor() { }

  @Input() label: string = '';
  @Input() icon: string = '';
  @Input() route: string = '';
  @Input() action?: () => void;
}
