import { Component, Injectable, OnInit } from '@angular/core';
import { AuthService } from './../auth/auth.service';
import { FilesService } from './files.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FilesFilterService } from './files-filter.service';
import { ConfirmationDialogService } from '../confirmation-dialog/confirmation-dialog.service';
import { Router, ActivatedRoute } from '@angular/router';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})



export class FilesComponent implements OnInit {

  constructor(public auth: AuthService, public upload: Upload, public confirm: ConfirmationDialogService, public filter: FilesFilterService, private service: FilesService, private router: Router, private route: ActivatedRoute) { }
  public files: any = [];
  errors: any = [];

  ngOnInit(): void {

    
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(["/auth/login"]);
      return;
    }

    this.getFiles();

  }

  public async delete(fileName: string) {
    this.confirm.openConfirmDeletionBox();

    if (await this.confirm.getResult()) {
      alert("Delete " + fileName);
      this.service.delete(fileName)
      .subscribe(() => {
        this.getFiles();
      },
      (errorResponse) => {
        
        console.error(errorResponse.error['error']);
      });
    }
  }

  public download(fileName: string) {

    this.service.download(fileName)
    .subscribe(x => {

      let blob:any = new Blob([x], { type: "application/octet-stream" });
			const url = window.URL.createObjectURL(blob);

			saveAs(blob, fileName);
  })
  }

  public getFiles() {
    this.service.getAllFiles()
    .subscribe((files) => {
      this.files = files;
     },
      (errorResponse) => {
        if (errorResponse.status === 404) {
          this.files = [];
        }
        else {
          console.error(errorResponse.error['error']);
        }
      });
  }

  public getFileSize(size: number) {
    return this.service.sizeToString(size);
  }

  public uploadFile() {
    
    this.service.upload(this.upload.file).subscribe(() => {
      this.getFiles();
    },
    (errorResponse) => {

      console.error(errorResponse.error['error']);
    });
  }
}

@Injectable({
  providedIn: 'root'
})
export class Upload {

  public form: FormGroup = new FormGroup ({
    _file: new FormControl('', Validators.required)
  });

  public file: File;

  public onchange(event) {
    this.file = event.target.files[0];
  }

  
}