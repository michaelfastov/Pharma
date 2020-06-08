import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, OnDestroy } from '@angular/core';
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
import { DatePipe } from '@angular/common';
import { RatingsService } from '../shared/services/ratings.service';
import { DoctorRating } from '../shared/models/doctor-rating';
import { HospitalsService } from '../shared/services/hospitals.service';
import { Hospital } from '../shared/models/hospital';
import { Reception } from '../shared/models/reception';
import { UserReception } from '../shared/models/user-reception';
import { PatientReception } from '../shared/models/patient-reception';


import { Subscription } from 'rxjs';
import { UserTypeService } from '../shared/services/user-type.service'
import { FormBuilder, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-reception',
  templateUrl: './reception.component.html',
  styleUrls: ['./reception.component.css']
})
export class ReceptionComponent implements OnInit {
  private userTypeSubscription: Subscription;
  userType = "";

  receptionFormGroup = this._fb.group({
    category: [null, Validators.required],
    hospital: [null, Validators.required],
    doctor: [null, Validators.required],
    date: [null, Validators.required],
    time: [null, Validators.required],
  });

  payForm = this._fb.group({
    data: [null, Validators.required],
    signature: [null, Validators.required],
  });



  public doctors: Doctor[] = [];
  public categories: DoctorRating[];
  public availableHours: string[];
  public hospitals: Hospital[];
  public liqPayModel: any;

  public doctorId: number = -1;

  public selectedDoctor: Doctor;
  public selectedCategory: string;
  public selectedHospital: Hospital;
  public selectedDate: string;
  public selectedTime: string;
  public doctorReceptions: UserReception[] = [];
  public patientReceptions: PatientReception[] = [];

  date = new FormControl(new Date());
  serializedDate = new FormControl((new Date()).toISOString());

  constructor(private _cd: ChangeDetectorRef,
    private _fb: FormBuilder, private userTypeService: UserTypeService, private _avRoute: ActivatedRoute, public http: Http, private _hospitalsService: HospitalsService, private _ratingsService: RatingsService, private datePipe: DatePipe, private _router: Router, private _receptionService: ReceptionService, private _doctorService: DoctorService, private _Activatedroute: ActivatedRoute) {
    this.doctorId = this._avRoute.snapshot.params["doctorId"];
  }

  ngOnInit() {
    this.userTypeSubscription = this.userTypeService.userType.subscribe(m => {
      this.userType = m;
      console.log(this.userType)
      if (this.userType == 'Doctor') {
        this.getDoctorsReceptions();
      }
      if (this.userType == 'Patient') {
        this.getCategories();
        this.getPatientsReceptions();
      }
      if (this.userType == 'Patient' && this.doctorId != undefined) {
        this._doctorService.GetDoctorById(this.doctorId).subscribe(data => {
          this.receptionFormGroup.get('category').setValue(data.specialization);
          this.selectedCategory = data.specialization;
          this.onCategorychange(this.selectedCategory);
          this.receptionFormGroup.get('doctor').setValue(data);
          this.selectedDoctor = data;
          this.doctors.push(this.selectedDoctor);
          this.GetDoctorsHospitals(this.selectedDoctor.doctorId);
          //this.receptionFormGroup.controls.category.setValue(data.specialization);

          // this.selectedDoctor = data;
          // this.selectedCategory = data.specialization;
        },
          error => {
            console.log(error);
          });
      }
    });

    // this.userType = localStorage.getItem('user_type');
    // if (this.userType == 'Doctor') {
    //   //this.GetDoctorsPatients();
    // }
    // if (this.userType == 'Patient') {
    //   //this.GetPatientsDocuments();
    // }//this.userType == 'Patient' && 
    // if (this.userType == 'Patient' && this.doctorId != -1) {
    //   this._doctorService.GetDoctorById(this.doctorId).subscribe(data => {
    //     debugger;
    //     this.receptionFormGroup.get('category').setValue(data.specialization);
    //     this.selectedCategory = data.specialization;
    //     this.onCategorychange(this.selectedCategory);
    //     this.receptionFormGroup.get('doctor').setValue(data);
    //     this.selectedDoctor = data;
    //     this.doctors.push(this.selectedDoctor);
    //     this.GetDoctorsHospitals(this.selectedDoctor.doctorId);
    //     //this.receptionFormGroup.controls.category.setValue(data.specialization);

    //     // this.selectedDoctor = data;
    //     // this.selectedCategory = data.specialization;
    //   },
    //     error => {
    //       console.log(error);
    //     });
    // }
  }
  // switchLanguage(language: string) {
  //   this.translate.use(language);
  // }

  getDoctorsReceptions() {
    this._receptionService.GetDoctorsReceptions().subscribe(data => {
      this.doctorReceptions = data;
    },
      error => {
        console.log(error);
      });
  }

  getPatientsReceptions() {
    this._receptionService.GetPatientsReceptions().subscribe(data => {
      this.patientReceptions = data;
      console.log(this.patientReceptions)
    },
      error => {
        console.log(error);
      });
  }

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

  GetDoctorsHospitals(doctorId) {
    this._doctorService.GetDoctorsHospitals(doctorId).subscribe(data => {
      this.hospitals = data;
    },
      error => {
        console.log(error);
      });
  }

  onDoctorChange(doctor) {
    this.selectedDoctor = doctor;

    this._receptionService.GetLiqPayModel(doctor.doctorId).subscribe(data => {
      this.liqPayModel = data;
    },
      error => {
        console.log(error);
      });
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

  pay() {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    // Website you wish to allow to connect
    headers.append('Access-Control-Allow-Origin', 'https://www.liqpay.ua/api');

    // Request methods you wish to allow
    headers.append('Access-Control-Allow-Methods', 'GET, POST');

    // Request headers you wish to allow
    //res.setHeader('Access-Control-Allow-Headers', 'X-Requested-With,content-type');

    // Set to true if you need the website to include cookies in the requests sent
    // to the API (e.g. in case you use sessions)
    //  res.setHeader('Access-Control-Allow-Credentials', true);
    // this.payForm.value.data = this.data;
    // this.payForm.value.signature = this.signature;
    console.log(JSON.stringify(this.liqPayModel))

    this.http.post('https://www.liqpay.ua/api/3/checkout', JSON.stringify(this.liqPayModel), { headers }).subscribe();
  }
}
