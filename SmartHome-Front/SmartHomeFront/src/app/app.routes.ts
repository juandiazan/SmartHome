// Components
import { Routes } from '@angular/router';
import { UserLoginComponent } from './components/user-login/user-login.component'; // Importo el componente de login de usuarios
import {UserAccountsListComponent } from './components/user-accounts-list/user-accounts-list.component'; // Importo la lista de usuarios
import { DeviceListComponent } from './components/device-list/device-list.component';
import { DeviceTypesListComponent } from './components/device-types-list/device-types-list.component';
import { SidebarComponent } from './components/sidebar/sidebar.component';
import { AuthGuard } from './components/auth-guard/auth-guard.component';
import { NoAuthGuard } from './components/auth-guard/no-auth-guard.component';
import { MainPageComponent } from './components/main-page/main-page.component';
import { RegisterAsHomeOwnerComponent } from './components/register-as-home-owner/register-as-home-owner.component';
import { DeleteUserAccountListItemComponent } from './components/delete-user-account-list-item/delete-user-account-list-item.component';
import { NotificationListComponent } from './components/notification-list/notification-list.component';
import { AddRoomToHomeComponent } from './components/add-room-to-home/add-room-to-home.component';
import { AddDeviceToRoomComponent } from './components/add-device-to-room/add-device-to-room.component';
import { DeviceImporterFormComponent } from './components/device-importer-form/device-importer-form.component';

// Views
import { AdminDeleteAdminsAccountsComponent } from './views/admin-views/admin-delete-admins-accounts/admin-delete-admins-accounts.component';
import { AdminRegisterUserFormComponent } from './views/admin-views/admin-register-user-form/admin-register-user-form.component'; 
import { AdminListCompanysComponent } from './views/admin-views/admin-list-companys/admin-list-companys.component'; 
import { CompanyOwnerRegisterCompanyComponent } from './views/company-owner-views/company-owner-register-company/company-owner-register-company.component';
import { RegiterHomeOwnerViewComponent } from './views/login-register-views/regiter-home-owner-view/regiter-home-owner-view.component';
import { CompanyOwnerRegisterDeviceViewComponent } from './views/company-owner-views/company-owner-register-device-view/company-owner-register-device-view.component';
import { HomeOwnerListHomeMembersComponent } from './views/home-owner-views/home-owner-list-home-members/home-owner-list-home-members.component';
import { HomeOwnerMemberNotificationsComponent } from './views/home-owner-views/home-owner-member-notifications/home-owner-member-notifications.component';
import { HomeOwnerAddMemberToHomeComponent } from './views/home-owner-views/home-owner-add-member-to-home/home-owner-add-member-to-home.component';
import { HomeOwnerSeeHomeDevicesComponent } from './views/home-owner-views/home-owner-see-home-devices/home-owner-see-home-devices.component';
import { CreateHomeFormComponent } from './components/create-home-form/create-home-form.component';
import { LayoutComponent } from './components/layout-component/layout-component.component';
import { MainViewComponent } from './views/main-view/main-view.component';
import { HomeOwnerAddDeviceToHomeComponent } from './views/home-owner-views/home-owner-add-device-to-home/home-owner-add-device-to-home.component';
import { CustomDeviceNameComponent } from './components/custom-device-name/custom-device-name.component';

export const routes: Routes = [
    { path: '', redirectTo: 'mainview', pathMatch: 'full' },
    // Rutas principales (sin sidebar)
    { path: 'main', component: MainPageComponent, canActivate: [NoAuthGuard] },
    { path: 'register', component: RegiterHomeOwnerViewComponent, canActivate: [NoAuthGuard] },
    { path: 'login', component: UserLoginComponent, canActivate: [NoAuthGuard] },

    // Rutas con layout persistente (con sidebar)
    {
        path: '',
        component: LayoutComponent, // El LayoutComponent será el contenedor de las rutas con sidebar
        canActivate: [AuthGuard],
        children: [
            { path: 'mainview', component: MainViewComponent },
            { path: 'home', component: MainViewComponent },

             // Administrator actions
            { path: 'create-user', component: AdminRegisterUserFormComponent },
            { path: 'list-users', component: UserAccountsListComponent },
            { path: 'delete-users', component: AdminDeleteAdminsAccountsComponent },
            { path: 'delete-users/:id', component: DeleteUserAccountListItemComponent },
            { path: 'list-companies', component: AdminListCompanysComponent },
            
            // Company Owner actions
            { path: 'register-company', component: CompanyOwnerRegisterCompanyComponent },
            { path: 'register-device', component: CompanyOwnerRegisterDeviceViewComponent },
            { path: 'importers', component: DeviceImporterFormComponent},
            

            // Home Owner actions
            { path: 'create-home', component: CreateHomeFormComponent },
            { path: 'add-member', component: HomeOwnerAddMemberToHomeComponent },
            { path: 'add-device', component: HomeOwnerAddDeviceToHomeComponent },
            { path: 'add-room', component: AddRoomToHomeComponent },
            { path: 'add-device-to-room', component: AddDeviceToRoomComponent },
            { path: 'list-members', component: HomeOwnerListHomeMembersComponent },
            { path: 'home-devices', component: HomeOwnerSeeHomeDevicesComponent },
            { path: 'grant-permissions', component: HomeOwnerMemberNotificationsComponent },
            { path: 'notifications', component:  NotificationListComponent},
            { path: 'modify-device-name', component:  CustomDeviceNameComponent},
            
            // General actions
            { path: 'devices', component: DeviceListComponent },
            { path: 'device-types', component: DeviceTypesListComponent },
            { path: 'register-as-home-owner', component: RegisterAsHomeOwnerComponent }
        ]
    },

    // Redirections in case of no route match
    { path: '**', redirectTo: 'main', pathMatch: 'full' }
];
