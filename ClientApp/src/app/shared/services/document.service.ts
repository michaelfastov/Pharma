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
import { HttpClient, HttpEvent, HttpEventType, HttpHeaders, HttpParams, HttpRequest } from '@angular/common/http';
import * as FileSaver from 'file-saver';

@Injectable({
    providedIn: 'root'
})
export class DocumentService {
    myAppUrl: string = "";
    constructor(private http: HttpClient, private _http: Http, private configService: ConfigService) {
        this.myAppUrl = configService.getApiURI();
    }

    GetPatientsDocuments() {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = localStorage.getItem('auth_token');
        headers.append('Authorization', `Bearer ${authToken}`);
        return this._http.get(this.myAppUrl + "/Documents/GetPatientDocuments", { headers }).map((response: Response) => response.json()).catch(this.errorHandler);
    }

    GetFileByDocumentId(documentId: number) {
        return this.getFile(this.myAppUrl + "/Documents/GetFileByDocumentId/" + documentId);
    }

    getFile(url: string) {
        let headers = new HttpHeaders();
        headers.append('Content-Type', 'application/json');
        let authToken = localStorage.getItem('auth_token');
        headers.append('Authorization', `Bearer ${authToken}`);

        let req = new HttpRequest('GET', url, {
            reportProgress: true,
            responseType: 'blob',
            headers: headers
        });

        this.http.request(req).subscribe((event: HttpEvent<any>) => {
            switch (event.type) {
                case HttpEventType.ResponseHeader:
                    if (!event.ok) {

                    }
                    break;
                case HttpEventType.Response:
                    let blob = new Blob([event.body], { type: event.body.type });
                    let fileName = event.headers.get('content-disposition').split(';').pop().substring(18);
                    FileSaver.saveAs(blob, fileName);
            }
        });
    }

    GetDocumentsByPatientId(patientId: number) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = localStorage.getItem('auth_token');
        headers.append('Authorization', `Bearer ${authToken}`);
        return this._http.get(this.myAppUrl + "/Documents/GetDocumentsByPatientId/" + patientId, { headers }).map((response: Response) => response.json()).catch(this.errorHandler);
    }

    SaveDocument(document: any) {
        let headers = new Headers();
        headers.append('Content-Type', 'application/json');
        let authToken = localStorage.getItem('auth_token');
        headers.append('Authorization', `Bearer ${authToken}`);
        return this._http.post(this.myAppUrl + '/Documents', document, { headers }).map((response: Response) => response.json()).catch(this.errorHandler)
    }

    errorHandler(error: Response) {
        console.log(error);
        return Observable.throw(error.json().error || 'Server error');
    }
}