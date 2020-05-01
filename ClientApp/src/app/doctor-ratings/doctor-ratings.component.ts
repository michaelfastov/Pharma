import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-doctor-ratings',
  templateUrl: './doctor-ratings.component.html',
  styleUrls: ['./doctor-ratings.component.css']
})
export class DoctorRatingsComponent implements OnInit {
  public ratings: DoctorRating[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<DoctorRating[]>(baseUrl + 'api/DoctorRatings').subscribe(result => {
      this.ratings = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }

}

interface DoctorRating {
  doctorRatingId: number;
  name: string;
}

