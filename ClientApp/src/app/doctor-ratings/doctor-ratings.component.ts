import { Component, OnInit, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RatingsService } from '../shared/services/ratings.service';
import { DoctorRating } from '../shared/models/doctor-rating';

@Component({
  selector: 'app-doctor-ratings',
  templateUrl: './doctor-ratings.component.html',
  styleUrls: ['./doctor-ratings.component.css']
})
export class DoctorRatingsComponent implements OnInit {
  public ratings: DoctorRating[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, _ratingsService: RatingsService) {
    _ratingsService.GetDoctorRatings().subscribe(result => {
      this.ratings = result;
    }, error => console.error(error));
  }

  ngOnInit() {
  }
}



