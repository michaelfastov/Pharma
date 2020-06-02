import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class UserTypeService {

    private source: BehaviorSubject<string> = new BehaviorSubject(localStorage.getItem('user_type')); 
    public userType = this.source.asObservable();

    public setUserType(value: string) {
        localStorage.setItem('user_type', value);
        this.source.next(value);
    }
}