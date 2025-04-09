import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [ CommonModule, FormsModule ],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent {
  @Input() offset: number = 1;
  @Input() limit: number = 10;
  @Output() offsetChange = new EventEmitter<number>();
  @Output() limitChange = new EventEmitter<number>();

  currentLimit: number = this.limit;

  ngOnChanges() {
    this.currentLimit = this.limit;
  }

  incrementOffset() {
    this.offset++;
    this.offsetChange.emit(this.offset);
  }

  decrementOffset() {
    if (this.offset > 1) {
      this.offset--;
      this.offsetChange.emit(this.offset);
    }
  }

  updateLimit() {
    this.limit = this.currentLimit;
    this.limitChange.emit(this.limit);
  }
}
