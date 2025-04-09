import { Component, Output, EventEmitter } from '@angular/core';
import { ModelValidator } from '../../models/ModelValidatorResponse';
import { CommonModule } from '@angular/common';
import { CompanyService } from '../../services/company-service.service';

@Component({
  selector: 'app-company-model-validator-selector',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './company-model-validator-selector.component.html',
  styleUrls: ['./company-model-validator-selector.component.css']
})
export class CompanyModelValidatorSelectorComponent {
  modelValidators: ModelValidator[] = [];

  constructor(private companyService: CompanyService) {}

  @Output() selectionChange = new EventEmitter<string>();
  onSelectChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.selectionChange.emit(selectElement.value);
  }

  ngOnInit() {
    this.loadModelValidators();
  }

  loadModelValidators() {
    this.companyService.getModelValidators().subscribe(({
      next: (data) => this.modelValidators = data,
      error: (error) => alert(error.message)
    }));
  }
}
