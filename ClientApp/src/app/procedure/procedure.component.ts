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
import { Procedure } from '../shared/models/procedure';
import { PatientProcedure } from '../shared/models/patient-procedure';
import { ProcedureService } from '../shared/services/procedure.service';
import { FormControl, FormGroupDirective, NgForm, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';


export class CustomErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(control: FormControl | null, form: FormGroupDirective | NgForm | null): boolean {
    const isSubmitted = form && form.submitted;
    return !!(control && control.invalid && (control.dirty || control.touched || isSubmitted));
  }
}

@Component({
  selector: 'app-procedure',
  templateUrl: './procedure.component.html',
  styleUrls: ['./procedure.component.css']
})
export class ProcedureComponent implements OnInit {
  private userTypeSubscription: Subscription;

  userType = "";
  selectedPatient: Patient;
  procedures: Procedure[];
  patients: Patient[];
  patientProcedures: PatientProcedure[];

  procedureFormControl = new FormControl('', [
    Validators.required,
  ]);
  matcher = new CustomErrorStateMatcher();

  constructor(private userTypeService: UserTypeService, private doctorService: DoctorService, private procedureService: ProcedureService) { }

  ngOnInit() {
    this.userTypeSubscription = this.userTypeService.userType.subscribe(m => {
      this.userType = m;
      if (this.userType == 'Doctor') {
        this.GetDoctorsPatients();
      }
      if (this.userType == 'Patient') {
        this.GetPatientsProcedures();
      }
    });

    // this.userType = localStorage.getItem('user_type');
    // if (this.userType == 'Doctor') {
    //   this.GetDoctorsPatients();
    // }
    // if (this.userType == 'Patient') {
    //   this.GetPatientsProcedures();
    // }
  }

  ngOnDestroy() {
    //this.userTypeSubscription.unsubscribe();
  }

  GetDoctorsPatients() {
    this.doctorService.GetDoctorsPatients().subscribe(data => {
      this.patients = data;
    },
      error => {
        console.log(error);
      });
  }

  GetPatientsProcedures() {
    this.procedureService.GetPatientsProcedures().subscribe(data => {
      this.patientProcedures = data;
    },
      error => {
        console.log(error);
      });
  }

  onPatientChange(patient) {
    this.selectedPatient = patient;
    this.procedureService.GetProceduresByPatientId(patient.patientId).subscribe(data => {
      this.procedures = data;
    },
      error => {
        console.log(error);
      });
  }

  submitProcedure() {
    var procedure: Procedure = {
      ProcedureId: 0,
      PatientId: this.selectedPatient.patientId,
      DoctorId: -1,
      Name: this.procedureFormControl.value,
      Price: -1,
      Category: ""
    }

    this.procedureService.SaveProcedure(procedure).subscribe((data) => {
      // this._router.navigate(['/somwhere']);  
      this.onPatientChange(this.selectedPatient);
    }, error => {
      console.log(error)
    })
  }
}
