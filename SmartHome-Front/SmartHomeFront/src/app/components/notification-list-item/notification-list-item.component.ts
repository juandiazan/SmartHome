import { Component } from '@angular/core';
import { Notification } from '../../models/Notification';
import { Input } from '@angular/core';

@Component({
  selector: 'app-notification-list-item',
  standalone: true,
  imports: [],
  templateUrl: './notification-list-item.component.html',
  styleUrl: './notification-list-item.component.css'
})
export class NotificationListItemComponent {
  @Input() notification!:Notification;
}
