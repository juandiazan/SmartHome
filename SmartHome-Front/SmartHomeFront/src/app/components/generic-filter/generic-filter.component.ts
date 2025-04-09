import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { DeviceTypesService } from '../../services/device-types.service';
import { Device } from '../../models/Device';

@Component({
  selector: 'app-generic-filter',
  standalone: true,
  imports: [FormsModule, CommonModule],
  templateUrl: './generic-filter.component.html',
  styleUrls: ['./generic-filter.component.css']
})
export class GenericFilterComponent<T = string> {
  @Input() label: string = 'Filter';
  @Input() type: string = 'text';
  @Input() value: T = '' as T;
  @Output() valueChange = new EventEmitter<T>();

  constructor (private deviceTypeService: DeviceTypesService) { }

  currentValue: T = this.value;
  options: string[] = [];

  ngOnInit() {
    this.currentValue = this.value;

    if (this.type === 'select') {
      this.deviceTypeService.getAllDeviceTypes().subscribe((data: string[]) => {
        this.options = data;
      });
    }
  }
  
  ngOnChanges() {
    this.currentValue = this.value;
  }

  updateValue() {
    this.value = this.currentValue;
    this.valueChange.emit(this.value);
  }

  resetValue() {
    if (this.type === 'select' && this.options.length > 0) {
      this.currentValue = '' as T;
    } else {
      this.currentValue = '' as T;
    }
    this.updateValue();
  }
}
