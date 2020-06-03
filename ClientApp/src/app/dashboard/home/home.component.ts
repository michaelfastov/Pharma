import { Component, OnInit } from '@angular/core';

import { HomeDetails } from '../models/home.details.interface';
import { DashboardService } from '../services/dashboard.service';
import { UserTypeService } from '../../shared/services/user-type.service'
import { Doctor } from '../../shared/models/doctor'
import { PatientViewModel } from '../../shared/models/patient-view-model'
import { Subscription } from 'rxjs';

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

  constructor(private dashboardService: DashboardService, private userTypeService: UserTypeService) { }

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
}
