import { Component, Input } from '@angular/core';
import { CompanyResponse } from '../../models/CompanyResponse';

@Component({
  selector: 'app-company-list-item',
  standalone: true,
  imports: [],
  templateUrl: './company-list-item.component.html',
  styleUrl: './company-list-item.component.css'
})
export class CompanyListItemComponent {
  @Input() company!:CompanyResponse;
}
