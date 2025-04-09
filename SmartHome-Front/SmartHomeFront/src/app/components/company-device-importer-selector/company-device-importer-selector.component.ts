import { Component, Output, EventEmitter  } from '@angular/core';
import { CommonModule} from '@angular/common';
import { DeviceImporter } from '../../models/DeviceImporterResponse';
import { CompanyService } from '../../services/company-service.service';

@Component({
  selector: 'app-company-device-importer-selector',
  standalone: true,
  imports: [
    CommonModule
  ],
  templateUrl: './company-device-importer-selector.component.html',
  styleUrl: './company-device-importer-selector.component.css'
})
export class CompanyDeviceImporterSelectorComponent {
  deviceImporters : DeviceImporter[] = [];
  
  constructor(private companyService: CompanyService) {}

  @Output() selectionChange = new EventEmitter<string>();
  onSelectChange(event: Event): void {
    const selectElement = event.target as HTMLSelectElement;
    this.selectionChange.emit(selectElement.value);
  }

  ngOnInit() {
    this.loadDeviceImporters();
  }

  loadDeviceImporters() {
    this.companyService.getDeviceImporters().subscribe(({
      next: (data) => this.deviceImporters = data,
      error: (error) => alert(error.message)
    }));
  }
}

