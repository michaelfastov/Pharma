import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserTypeService } from '../shared/services/user-type.service'
import { UserService } from '../shared/services/user.service'
import { LanguageService } from '../shared/services/language.service'
import { TranslateService } from '@ngx-translate/core';

import { Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  isExpanded = false;
  userType = "";
  selectedLanguage = "";
  private userTypeSubscription: Subscription;
  private translateSubscription: Subscription;

  constructor(private userTypeService: UserTypeService, private userService: UserService, private translate: TranslateService, private language: LanguageService) { }

  ngOnInit() {
    this.userTypeSubscription = this.userTypeService.userType.subscribe(m => {
      this.userType = m;
    });
    this.translateSubscription = this.language.language.subscribe(m => {
      this.switchLanguage(m);
    });
  }

  ngOnDestroy() {
    this.userTypeSubscription.unsubscribe();
    this.translateSubscription.unsubscribe();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this.userType = "No User";
    this.userService.logout();
  }

  setLanguage(lang) {
    this.language.setLanguage(lang);
  }

  switchLanguage(language: string) {
    this.translate.use(language);
  }
}
