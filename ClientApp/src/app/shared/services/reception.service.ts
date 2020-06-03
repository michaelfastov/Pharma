import {
  Injectable,
  Inject
} from '@angular/core';
import {
  Http,
  Response,
  Headers
} from '@angular/http';
import {
  Observable
} from 'rxjs/Observable';
import {
  Router
} from '@angular/router';
import { ConfigService } from '../../shared/utils/config.service';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/throw';

@Injectable({
  providedIn: 'root'
})
export class ReceptionService {
  myAppUrl: string = "";
  constructor(private _http: Http, private configService: ConfigService) {
    this.myAppUrl = configService.getApiURI();
  }

  GetLiqPayModel(doctorId: number) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return this._http.get(this.myAppUrl + "/Receptions/GetLiqPayModel/" + doctorId, { headers }).map((response: Response) => response.json()).catch(this.errorHandler);
  }

  GetDoctorsReceptions() {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return this._http.get(this.myAppUrl + "/Receptions/GetDoctorsReceptions", { headers }).map((response: Response) => response.json()).catch(this.errorHandler);
  }

  GetPatientsReceptions() {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return this._http.get(this.myAppUrl + "/Receptions/GetPatientsReceptions", { headers }).map((response: Response) => response.json()).catch(this.errorHandler);
  }


  GetReceptionsByDoctorId(doctorId: number) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return this._http.get(this.myAppUrl + "/Receptions/GetReceptionsByDoctorId/" + doctorId, { headers }).map((response: Response) => response.json()).catch(this.errorHandler);
  }

  GetAvailableReceptionTime(doctorId: number, date: string) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return this._http.get(this.myAppUrl + "/Receptions/GetAvailableReceptionTime/" + doctorId + "/" + date, { headers }).map((response: Response) => response.json()).catch(this.errorHandler);
  }

  saveReception(reception: any) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');
    let authToken = localStorage.getItem('auth_token');
    headers.append('Authorization', `Bearer ${authToken}`);
    return this._http.post(this.myAppUrl + '/Receptions', reception, { headers }).map((response: Response) => response.json()).catch(this.errorHandler)
  }

  errorHandler(error: Response) {
    console.log(error);
    return Observable.throw(error.json().error || 'Server error');
  }
}  