import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/modules/shared.module';
import { RouterModule } from '@angular/router';

import { UserService } from '../shared/services/user.service';


import { RegistrationFormComponent } from './registration-form/registration-form.component';
import { LoginFormComponent } from './login-form/login-form.component';


@NgModule({
  imports: [
    CommonModule, FormsModule, SharedModule,
    RouterModule.forChild([
      { path: 'register', component: RegistrationFormComponent },
      { path: 'login', component: LoginFormComponent }
    ])
  ],
  declarations: [RegistrationFormComponent, LoginFormComponent],
  providers: [UserService]
})
export class AccountModule { }