import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserTypeService {
  private userTypeSource = new BehaviorSubject<string>('administrator');
  currentUserType$ = this.userTypeSource.asObservable();

  changeUserType(type: string) {
    this.userTypeSource.next(type);
  }

  getUserType() {
    return this.userTypeSource.getValue(); 
  }
}