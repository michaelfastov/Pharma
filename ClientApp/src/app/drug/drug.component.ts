import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

import { UserTypeService } from '../shared/services/user-type.service'
import {
  Http,
  Headers
} from '@angular/http';
import {
  ReceptionService
} from '../shared/services/reception.service'
import {
  Doctor
} from '../shared/models/doctor'
import {
  Router,
  ActivatedRoute
} from '@angular/router';
//import { TranslateService } from '@ngx-translate/core';
import { DatePipe } from '@angular/common';
import { RatingsService } from '../shared/services/ratings.service';
import { DoctorRating } from '../shared/models/doctor-rating';
import { DoctorService } from '../shared/services/doctor.service';
import { Patient } from '../shared/models/patient';
import { Drug } from '../shared/models/drug';
import { DrugService } from '../shared/services/drug.service';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';


export class CustomErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-drug',
  templateUrl: './drug.component.html',
  styleUrls: ['./drug.component.css']
})
export class DrugComponent implements OnInit {
  private userTypeSubscription: Subscription;

  userType = "";
  selectedPatient: Patient;
  drugs: Drug[];
  patients: Patient[];

  drugFormControl = new FormControl('', [
    Validators.required,
  ]);
  matcher = new CustomErrorStateMatcher();

  constructor(private userTypeService: UserTypeService, private doctorService: DoctorService, private drugService: DrugService) { }

  ngOnInit() {
    this.userTypeSubscription = this.userTypeService.userType.subscribe(m => {
      this.userType = m;
      console.log(this.userType);

      if (this.userType == 'Doctor') {
        this.GetDoctorsPatients();
      }
    });
  }

  ngOnDestroy() {
    this.userTypeSubscription.unsubscribe();
  }

  GetDoctorsPatients() {
    this.doctorService.GetDoctorsPatients().subscribe(data => {
      this.patients = data;
    },
      error => {
        console.log(error);
      });
  }

  onPatientChange(patient) {
    this.selectedPatient = patient;
    this.drugService.GetDrugsByPatientId(patient.patientId).subscribe(data => {
      this.drugs = data;
    },
      error => {
        console.log(error);
      });
  }

  submitDrug() {
    var drug: Drug = {
      DrugId: 0,
      PatientId: this.selectedPatient.patientId,
      Name: this.drugFormControl.value,
      Price: -1,
      Category: ""
    }

    this.drugService.SaveDrug(drug).subscribe((data) => {
      // this._router.navigate(['/somwhere']);  
      this.onPatientChange(this.selectedPatient);
    }, error => {
      console.log(error)
    })
  }
}
