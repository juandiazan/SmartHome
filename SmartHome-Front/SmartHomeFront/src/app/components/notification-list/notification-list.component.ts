import { Component } from '@angular/core';
import { NotificationService } from '../../services/notification.service';
import { Notification } from '../../models/Notification';
import { NotificationListItemComponent } from "../notification-list-item/notification-list-item.component";
import { CommonModule } from '@angular/common';
import { GenericFilterComponent } from '../generic-filter/generic-filter.component';

@Component({
  selector: 'app-notification-list',
  standalone: true,
  imports: [NotificationListItemComponent, CommonModule, GenericFilterComponent],
  templateUrl: './notification-list.component.html',
  styleUrl: './notification-list.component.css'
})
export class NotificationListComponent {
  deviceType : string = "";
  creationDate : string = "";
  wasRead : boolean | null = null;
  
  notifications: Notification[] = [];
  
  constructor(private notificationService: NotificationService) { }
  
  ngOnInit(){
    this.loadNotifications();
  }

  setDeviceType(newDeviceType: string) {
    this.deviceType = newDeviceType;
    this.loadNotifications();
  }

  setCreationDate(newCreationDate: string) {
    this.creationDate = newCreationDate;
    this.loadNotifications();
  }

  setWasRead(newWasRead: boolean | null) {
    this.wasRead = newWasRead;
    this.loadNotifications();
  }

  resetWasRead() {
    this.wasRead = null;
    this.loadNotifications();
  }

  loadNotifications(){
    this.notificationService.getNotifications(this.deviceType, this.creationDate, this.wasRead).subscribe(({
      next: (data) => this.notifications = data,
      error: (error) => console.error(error)
    }));
  }
}
