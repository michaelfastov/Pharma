import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class LanguageService {

    private source: BehaviorSubject<string> = new BehaviorSubject("ukr"); 
    public language = this.source.asObservable();

    public setLanguage(value: string) {
        this.source.next(value);
    }
}