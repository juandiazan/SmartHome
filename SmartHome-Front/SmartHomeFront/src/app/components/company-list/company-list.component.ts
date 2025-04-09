import { Component } from '@angular/core';
import { CompanyListItemComponent } from '../company-list-item/company-list-item.component';
import { CompanyResponse } from '../../models/CompanyResponse';
import { CompanyService } from '../../services/company-service.service';
import { PaginationComponent } from '../pagination/pagination.component';
import { GenericFilterComponent } from '../generic-filter/generic-filter.component';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [
    CompanyListItemComponent,
    PaginationComponent,
    GenericFilterComponent,
    CommonModule
  ],
  templateUrl: './company-list.component.html',
  styleUrl: './company-list.component.css'
})
export class CompanyListComponent {
  offset : number = 1;
  limit : number = 10;
  companyName : string = "";
  ownerName : string = "";
  
  setOffset(newOffset: number) {
    this.offset = newOffset;
    this.loadCompanies();
  }
  
  setLimit(newLimit: number) {
    this.limit = newLimit;
    this.loadCompanies();
  }

  setCompanyName(newName: string) {
    this.companyName = newName;
    this.loadCompanies();
  }

  setOwnerName(newName: string) {
    this.ownerName = newName;
    this.loadCompanies();
  }

 apiCompanies: CompanyResponse[] = [];

 constructor(private companyService: CompanyService) { }
  
 ngOnInit(){
   this.loadCompanies();
 }

 loadCompanies(){
   this.companyService.getCompanies(this.offset, this.limit, this.companyName, this.ownerName).subscribe(({
     next: (data) => this.apiCompanies = data,
     error: (error) => alert(error.message)
   }));
 }
}
