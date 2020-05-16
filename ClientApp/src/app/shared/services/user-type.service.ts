import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class UserTypeService {

    private source: BehaviorSubject<string> = new BehaviorSubject('No User'); 
    public userType = this.source.asObservable();

    public setUserType(value: string) {
        this.source.next(value);
    }
}