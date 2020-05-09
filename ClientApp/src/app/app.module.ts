import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { DoctorRatingsComponent } from './doctor-ratings/doctor-ratings.component';
/* Account Imports */
import { AccountModule } from './account/account.module';
/* Dashboard Imports */
import { DashboardModule } from './dashboard/dashboard.module';
import { HttpModule, XHRBackend } from '@angular/http';
import { AuthenticateXHRBackend } from './authenticate-xhr.backend';
import { ConfigService } from './shared/utils/config.service';
import { ReceptionComponent } from './reception/reception.component';
import { ReceptionService } from './shared/services/reception.service';
import { DoctorService } from './shared/services/doctor.service';
import { RatingsService } from './shared/services/ratings.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material';
import { MatInputModule } from '@angular/material';
import { DatePipe } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { RatingComponent } from './rating/rating.component';
import { MatTableModule } from '@angular/material/table';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    DoctorRatingsComponent,
    ReceptionComponent,
    RatingComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AccountModule,
    DashboardModule,
    HttpModule,
    MatFormFieldModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'doctor-ratings', component: DoctorRatingsComponent },
      { path: 'reception', component: ReceptionComponent },
      { path: 'rating/:ratingName', component: RatingComponent }
    ]),
    BrowserAnimationsModule
  ],
  providers: [ConfigService, {
    provide: XHRBackend,
    useClass: AuthenticateXHRBackend
  },
    DatePipe],
  bootstrap: [AppComponent]
})
export class AppModule { }
