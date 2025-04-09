import { Component } from '@angular/core';
import { CompanyRegisterFormComponent } from '../../../components/company-register-form/company-register-form.component';


@Component({
  selector: 'app-company-owner-register-company',
  standalone: true,
  imports: [
    CompanyRegisterFormComponent
  ],
  templateUrl: './company-owner-register-company.component.html',
  styleUrl: './company-owner-register-company.component.css'
})
export class CompanyOwnerRegisterCompanyComponent {

}
