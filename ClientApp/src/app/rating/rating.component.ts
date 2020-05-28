import { Component, OnInit } from '@angular/core';
import { RatingsService } from '../shared/services/ratings.service';
import { DoctorRating } from '../shared/models/doctor-rating';
import { Rating } from '../shared/models/rating';
import {
  Router,
  ActivatedRoute
} from '@angular/router';

@Component({
  selector: 'app-rating',
  templateUrl: './rating.component.html',
  styleUrls: ['./rating.component.css']
})
export class RatingComponent implements OnInit {

  public ratings: Rating[];
  public ratingName: string;
  public displayedColumns: string[] = ['rankingPlace', 'doctorName'];

  constructor(private _avRoute: ActivatedRoute, private _ratingsService: RatingsService) {
    this.ratingName = this._avRoute.snapshot.params["ratingName"];
  }

  ngOnInit() {
    this._ratingsService.GetDoctorRatingsByCategory(this.ratingName).subscribe(result => {
      this.ratings = result;
    }, error => console.error(error));
  }
}
