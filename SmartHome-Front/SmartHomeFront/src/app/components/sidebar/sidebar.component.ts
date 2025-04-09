import { Router } from '@angular/router';
import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SidebarItemComponent } from '../sidebar-item/sidebar-item.component';
import { MatIconModule } from '@angular/material/icon';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    SidebarItemComponent,
    MatIconModule
  ],
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  userRole: string | null = null;
  isSecondarySidebar: boolean = false;

  items: Array<{ label: string; icon: string; route: string; action?: (() => void); isLogout?: boolean }> = [];
  secondaryRoleItems: Array<{ label: string; icon: string; route: string }> = [];

  adminItems: Array<{ label: string; icon: string; route: string }> = 
  [
    { label: 'Create administrator or company owner', icon: 'bi-person-plus-fill', route: '/create-user' },
    { label: 'List companies', icon: 'bi-buildings', route: '/list-companies' },
    { label: 'List users', icon: 'bi-person-lines-fill', route: '/list-users' },
    { label: 'Delete user', icon: 'bi-person-dash-fill', route: '/delete-users' },
  ]; 
  
  companyOwnerItems: Array<{ label: string; icon: string; route: string }> =
  [
    { label: 'Create Company', icon: 'bi-building-add', route: '/register-company' },
    { label: 'Create Device', icon: 'bi-camera-video', route: '/register-device' },
    { label: 'Import devices', icon: 'bi-archive', route: '/importers' },
  ]; 

  homeOwnerItems: Array<{ label: string; icon: string; route: string }> = 
  [
    { label: 'Create Home', icon: 'bi-house-add-fill', route: '/create-home' },
    { label: 'Add device to home', icon: 'bi-file-plus', route: '/add-device' },
    { label: 'Add member to home', icon: 'bi-person-standing', route: '/add-member' },
    { label: 'Add room to home', icon: 'bi-house-gear', route: '/add-room' },
    { label: 'Add device to room', icon: 'bi-house-gear', route: '/add-device-to-room' },
    { label: 'Modify home device name', icon: 'bi-cpu', route: '/modify-device-name' }, 
    { label: 'List members of a home', icon: 'bi-person-lines-fill', route: '/list-members' },
    { label: 'List devices of a home', icon: 'bi-list-ul', route: '/home-devices' },
    { label: 'Update member notification permissions', icon: 'bi-person-gear', route: '/grant-permissions' },
    { label: 'My Notifications', icon: 'bi-bell-fill', route: '/notifications' },
  ];

  listDevices = { label: 'List available devices', icon: 'bi-list-ul', route: '/devices' };
  listDeviceTypes = { label: 'List devices types', icon: 'bi-list-ul', route: '/device-types' };
  activateHomeOwnerAccount = { label: 'Register as Home Owner', icon: 'bi-house-fill', route: '/register-as-home-owner' };
  logoutButton = { label: 'Logout', icon: 'bi-box-arrow-left', route: '/main', action: this.logout.bind(this), isLogout: true };

  
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {
    this.authService.role$.subscribe(role => {
      this.userRole = role;
      this.updateSidebar();
    });
  }

  private updateSidebar(): void {
    this.userRole = this.authService.getRole();

    const activateHomeOwnerIndex = this.items.findIndex(item => item.label === this.activateHomeOwnerAccount.label);
    if (activateHomeOwnerIndex !== -1) {
      this.items.splice(activateHomeOwnerIndex, 1);
    }

    if (this.userRole === 'administrator') {
      this.items = [...this.adminItems, this.listDevices, this.listDeviceTypes];
      this.items.push(this.logoutButton, this.activateHomeOwnerAccount);
    }
  
    if (this.userRole === 'company-owner') {
      this.items = [...this.companyOwnerItems, this.listDevices, this.listDeviceTypes];
      this.items.push(this.logoutButton, this.activateHomeOwnerAccount);
    }
  
    if (this.userRole === 'home-owner') {
      this.items = [...this.homeOwnerItems, this.listDevices, this.listDeviceTypes];
      this.items.push(this.logoutButton);
    }
  
    if (this.userRole === 'admin-home-owner') {
      this.items = [...this.adminItems, this.listDevices, this.listDeviceTypes, this.logoutButton];
      this.secondaryRoleItems = [...this.homeOwnerItems];
    }
  
    if (this.userRole === 'company-owner-home-owner') {
      this.items = [...this.companyOwnerItems, this.listDevices, this.listDeviceTypes, this.logoutButton];
      this.secondaryRoleItems = [...this.homeOwnerItems];
    }
  }

  showSecondarySidebar(): void {
    this.isSecondarySidebar = true;
  }

  showPrimarySidebar(): void {
    this.isSecondarySidebar = false;
  }

  logout(): void {
    localStorage.removeItem('token');
    localStorage.removeItem('userRole');
    this.authService.setRole("");
  }
}
