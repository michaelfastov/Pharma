import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { DocumentService } from '../shared/services/document.service';
import { Document } from '../shared/models/document';
import { Subscription } from 'rxjs';
import { Patient } from '../shared/models/patient';
import { DoctorService } from '../shared/services/doctor.service';

import { UserTypeService } from '../shared/services/user-type.service'
@Component({
  selector: 'app-document',
  templateUrl: './document.component.html',
  styleUrls: ['./document.component.css']
})
export class DocumentComponent implements OnInit {
  private userTypeSubscription: Subscription;
  userType = "";

  //public idInput = new FormControl('', Validators.compose([Validators.required, Validators.min(1)]));
  patients: Patient[];
  selectedPatient: Patient;

  exportFormGroup = this._fb.group({
    fileName: [null, Validators.required],
    file: [null, Validators.required]
  });

  constructor(private userTypeService: UserTypeService, private doctorService: DoctorService, 
    private _cd: ChangeDetectorRef,
    private _fb: FormBuilder,
    private documentService: DocumentService) {

  }

  ngOnInit() {
    this.userTypeSubscription = this.userTypeService.userType.subscribe(m => {
      this.userType = m;
      if (this.userType == 'Doctor') {
        this.GetDoctorsPatients();
      }
      if (this.userType == 'Patient') {
      //  this.GetPatientsProcedures();
      }
    });
  }

  ngOnDestroy() {
    this.userTypeSubscription.unsubscribe();
  }

  onPatientChange(patient) {
    this.selectedPatient = patient;
    // this.procedureService.GetProceduresByPatientId(patient.patientId).subscribe(data => {
    //   this.procedures = data;
    // },
    //   error => {
    //     console.log(error);
    //   });
  }

  GetDoctorsPatients() {
    this.doctorService.GetDoctorsPatients().subscribe(data => {
      this.patients = data;
    },
      error => {
        console.log(error);
      });
  }

  onFileChange(event) {
    const reader = new FileReader();

    if (event.target.files && event.target.files.length) {
      const [file] = event.target.files;
      reader.readAsDataURL(file);

      reader.onloadend = () => {
        this.exportFormGroup.patchValue({
          file: reader.result,
          fileName: file.name
        });
        this._cd.markForCheck();
      };
    }
  }

  // public download(): void {
  //   this.documentService.exportTestSuite(this.idInput.value);
  // }

  public upload(): void {
    var document: Document = {
      DocumentId: 0,
      PatientId: this.selectedPatient.patientId,
      DoctorId: -1,
      Name: this.exportFormGroup.value.fileName,
      Comments: "",
      File: this.exportFormGroup.value.file
    }

    this.documentService.SaveDocument(document).subscribe(
      () => {
        //this._alertService.success(`Test Suite '${this.exportFormGroup.get('fileName').value}' successfully imported`);
        //this.dialogRef.close();
      }
    );
  }
}
