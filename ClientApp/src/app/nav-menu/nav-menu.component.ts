import { Component, OnInit, OnDestroy } from '@angular/core';
import { UserTypeService } from '../shared/services/user-type.service'
import { UserService } from '../shared/services/user.service'

import { Subscription } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit, OnDestroy {
  isExpanded = false;
  userType = "";
  private userTypeSubscription: Subscription;

  constructor(private userTypeService: UserTypeService, private userService: UserService) { }

  ngOnInit() {
    this.userTypeSubscription = this.userTypeService.userType.subscribe(m => {
      this.userType = m;
    });
    //this.userType = localStorage.getItem('user_type');
  }

  ngOnDestroy() {
    this.userTypeSubscription.unsubscribe();
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout(){
    this.userType = "No User";
    this.userService.logout();
  }
}
