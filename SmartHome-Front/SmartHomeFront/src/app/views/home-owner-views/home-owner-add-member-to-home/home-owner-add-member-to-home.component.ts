import { Component } from '@angular/core';
import { AddMemberToHomeFormComponent } from '../../../components/add-member-to-home-form/add-member-to-home-form.component';


@Component({
  selector: 'app-home-owner-add-member-to-home',
  standalone: true,
  imports: [
    AddMemberToHomeFormComponent
  ],
  templateUrl: './home-owner-add-member-to-home.component.html',
  styleUrl: './home-owner-add-member-to-home.component.css'
})
export class HomeOwnerAddMemberToHomeComponent {

}
