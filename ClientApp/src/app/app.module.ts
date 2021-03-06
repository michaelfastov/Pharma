import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
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
import { DrugService } from './shared/services/drug.service';
import { ProcedureService } from './shared/services/procedure.service';
import { UserTypeService } from './shared/services/user-type.service';
import { LanguageService } from './shared/services/language.service';
import { UserService } from './shared/services/user.service';
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
import { DrugComponent } from './drug/drug.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProcedureComponent } from './procedure/procedure.component';
import { DocumentComponent } from './document/document.component';
import { MatIconModule } from '@angular/material/icon';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { HttpClient } from '@angular/common/http';

export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http);
}

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    CounterComponent,
    FetchDataComponent,
    DoctorRatingsComponent,
    ReceptionComponent,
    RatingComponent,
    DrugComponent,
    ProcedureComponent,
    DocumentComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
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
    MatIconModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      }
    }),
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'doctor-ratings', component: DoctorRatingsComponent },
      { path: 'reception', component: ReceptionComponent },
      { path: 'reception/:doctorId', component: ReceptionComponent },
      { path: 'rating/:ratingName', component: RatingComponent },
      { path: 'drug', component: DrugComponent },
      { path: 'procedure', component: ProcedureComponent },
      { path: 'document', component: DocumentComponent },
    ]),
    BrowserAnimationsModule
  ],
  providers: [ConfigService, {
    provide: XHRBackend,
    useClass: AuthenticateXHRBackend
  },
    DatePipe,
    UserService,
    UserTypeService,
    LanguageService,
    DrugService],
  bootstrap: [AppComponent]
})
export class AppModule { }
