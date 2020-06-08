import { Component, OnInit } from '@angular/core';

import { HomeDetails } from '../models/home.details.interface';
import { DashboardService } from '../services/dashboard.service';
import { UserTypeService } from '../../shared/services/user-type.service'
import { Doctor } from '../../shared/models/doctor'
import { PatientViewModel } from '../../shared/models/patient-view-model'
import { Subscription } from 'rxjs';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { Patient } from '../../shared/models/patient'
import { UserService } from '../../shared/services/user.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  private userTypeSubscription: Subscription;
  userType = "";
  doctor: Doctor;
  patient: PatientViewModel;
  isEdit = false;
  patientFormGroup = this._fb.group({
    name: [null, Validators.required],
    surname: [null, Validators.required],
    address: [null, Validators.required],
    phone: [null, Validators.required],
  });

  doctorFormGroup = this._fb.group({
    name: [null, Validators.required],
    surname: [null, Validators.required],
    phone: [null, Validators.required],
    specialization: [null, Validators.required],
    receptionPrice: [null, Validators.required],
  });

  constructor(private dashboardService: DashboardService, private userTypeService: UserTypeService, private _fb: FormBuilder, private userService: UserService) { }

  ngOnInit() {
    this.userTypeSubscription = this.userTypeService.userType.subscribe(userType => {
      this.userType = userType;
      if (this.userType == 'Doctor') {
        this.GetDoctor();
      }
      if (this.userType == 'Patient') {
        this.GetPatient();
      }
    })
  }

  GetDoctor() {
    this.dashboardService.GetDoctorHome().subscribe(data => {
      this.doctor = data;
    },
      error => {
        console.log(error);
      });
  }

  GetPatient() {
    this.dashboardService.GetPatientHome().subscribe(data => {
      this.patient = data;
    },
      error => {
        console.log(error);
      });
  }

  OpenPatientEdit() {
    this.isEdit = !this.isEdit;

    this.patientFormGroup.patchValue({
      name: this.patient.name,
      surname: this.patient.surname,
      address: this.patient.address,
      phone: this.patient.phone
    });
  }

  OpenDoctorEdit() {
    this.isEdit = !this.isEdit;

    this.doctorFormGroup.patchValue({
      name: this.doctor.name,
      surname: this.doctor.surname,
      phone: this.doctor.phone,
      specialization: this.doctor.specialization,
      receptionPrice: this.doctor.receptionPrice,
    });
  }

  EditPatient() {
    this.isEdit = !this.isEdit;

    var patient: Patient = {
      patientId: -1,
      name: this.patientFormGroup.get('name').value,
      surname: this.patientFormGroup.get('surname').value,
      dob: null,
      address: this.patientFormGroup.get('address').value,
      phone: this.patientFormGroup.get('phone').value
    }

    this.userService.PutPatient(patient).subscribe((data) => {
      this.GetPatient()
    }, error => {
      console.log(error)
    })
  }

  EditDoctor() {
    this.isEdit = !this.isEdit;

    var doctor: Doctor = {
      doctorId: -1,
      name: this.doctorFormGroup.get('name').value,
      surname: this.doctorFormGroup.get('surname').value,
      phone: this.doctorFormGroup.get('phone').value,
      specialization: this.doctorFormGroup.get('specialization').value,
      receptionPrice: this.doctorFormGroup.get('receptionPrice').value
    }

    this.userService.PutDoctor(doctor).subscribe((data) => {
      this.GetDoctor()
    }, error => {
      console.log(error)
    })
  }
}
