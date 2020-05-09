import { Component, OnInit, Inject } from '@angular/core';
import {
  Http,
  Headers
} from '@angular/http';
import {
  ReceptionService
} from '../shared/services/reception.service'
import {
  DoctorService
} from '../shared/services/doctor.service'
import {
  Doctor
} from '../shared/models/doctor'
import {
  Router,
  ActivatedRoute
} from '@angular/router';
//import { TranslateService } from '@ngx-translate/core';
import { FormControl } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { RatingsService } from '../shared/services/ratings.service';
import { DoctorRating } from '../shared/models/doctor-rating';
import { HospitalsService } from '../shared/services/hospitals.service';
import { Hospital } from '../shared/models/hospital';
import { Reception } from '../shared/models/reception';

@Component({
  selector: 'app-reception',
  templateUrl: './reception.component.html',
  styleUrls: ['./reception.component.css']
})
export class ReceptionComponent implements OnInit {
  public doctors: Doctor[];
  public categories: DoctorRating[];
  public availableHours: string[];
  public hospitals: Hospital[];

  public selectedDoctor: Doctor;
  public selectedCategory: string;
  public selectedHospital: Hospital;
  public selectedDate: string;
  public selectedTime: string;

  date = new FormControl(new Date());
  serializedDate = new FormControl((new Date()).toISOString());

  constructor(public http: Http, private _hospitalsService: HospitalsService, private _ratingsService: RatingsService, private datePipe: DatePipe, private _router: Router, private _receptionService: ReceptionService, private _doctorService: DoctorService, private _Activatedroute: ActivatedRoute) {
    //globals.TeamId=this._Activatedroute.snapshot.params['teamID'];
    // translate.setDefaultLang('en');
    this.getCategories();
  }

  // switchLanguage(language: string) {
  //   this.translate.use(language);
  // }

  getDoctors() {
    this._doctorService.GetDoctorsByHospitalIdAndCategory(this.selectedHospital.hospitalId, this.selectedCategory).subscribe(data => {
      this.doctors = data;
    },
      error => {
        console.log(error);
      });
  }

  getHospitals() {
    this._hospitalsService.GetHospitalsByDoctorsCategory(this.selectedCategory).subscribe(data => {
      this.hospitals = data;
    },
      error => {
        console.log(error);
      });
  }

  getCategories() {
    this._ratingsService.GetDoctorRatings().subscribe(data => {
      this.categories = data;
    }, error => console.error(error));
  }

  onDoctorChange(doctor) {
    this.selectedDoctor = doctor;
  }

  onCategorychange(category) {
    this.selectedCategory = category;
    this.getHospitals();
  }

  onHospitalchange(hospital) {
    this.selectedHospital = hospital;
    this.getDoctors();
  }

  OnDateChange(date) {
    this.selectedDate = this.datePipe.transform(date, 'yyyy-MM-dd');
    this._receptionService.GetAvailableReceptionTime(this.selectedDoctor.doctorId, this.selectedDate).subscribe(data => {
      this.availableHours = data;
      console.log(this.availableHours);
    },
      error => {
        console.log(error);
      });
  }

  onTimeChange(time) {
    this.selectedTime = time;
  }

  submitReception() {
    var reception: Reception = {
      ReceptionId: 0,
      PatientId: -1,
      DoctorId: this.selectedDoctor.doctorId,
      HospitalId: this.selectedHospital.hospitalId,
      Time: this.selectedTime,
      Duration: "00:30:00",
      Date: this.selectedDate,
      DayOfWeek: "",
      Address: this.selectedHospital.address,
      Purpose: this.selectedCategory,
      Result: "",
      Price: this.selectedDoctor.receptionPrice
    }
    
    this._receptionService.saveReception(reception).subscribe((data) => {
      // this._router.navigate(['/somwhere']);  
    }, error => {
      console.log(error)
    })
  }

  ngOnInit() {
  }
}
