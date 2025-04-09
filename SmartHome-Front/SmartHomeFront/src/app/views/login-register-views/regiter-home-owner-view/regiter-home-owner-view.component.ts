import { Component } from '@angular/core';
import { RegisterHomeownerFormComponent } from '../../../components/register-homeowner-form/register-homeowner-form.component';


@Component({
  selector: 'app-regiter-home-owner-view',
  standalone: true,
  imports: [
    RegisterHomeownerFormComponent
  ],
  templateUrl: './regiter-home-owner-view.component.html',
  styleUrl: './regiter-home-owner-view.component.css'
})
export class RegiterHomeOwnerViewComponent {

}
