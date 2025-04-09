import { Component } from '@angular/core';
import { CompanyListComponent } from '../../../components/company-list/company-list.component';
import { FormItemComponent } from '../../../components/form-item/form-item.component';

@Component({
  selector: 'app-admin-list-companys',
  standalone: true,
  imports: [
    CompanyListComponent
  ],
  templateUrl: './admin-list-companys.component.html',
  styleUrl: './admin-list-companys.component.css'
})
export class AdminListCompanysComponent {

}
