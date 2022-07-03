import { Component, Injectable, OnInit } from '@angular/core';
import { AuthService } from './../auth/auth.service';
import { FilesService } from './files.service';
import { ConfirmationDialogService } from '../confirmation-dialog/confirmation-dialog.service';
import { Router, ActivatedRoute } from '@angular/router';
import { saveAs } from 'file-saver';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})



export class FilesComponent implements OnInit {

  constructor(public auth: AuthService, public upload: Upload, public confirm: ConfirmationDialogService , private service: FilesService, private router: Router, private route: ActivatedRoute) { }
  public files: any = [];
  errors: any = [];

  ngOnInit(): void {

    
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(["/auth/login"]);
      return;
    }

    this.getFiles();

  }

  public download(fileName) {

    this.service.download(fileName)
    .subscribe(x => {

      let blob:any = new Blob([x], { type: "application/octet-stream" });
			const url = window.URL.createObjectURL(blob);
			//window.open(url);
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
}

@Injectable({
  providedIn: 'root'
})
export class Upload {

  file: File;

  public onChange(event) {
    this.file = event.target.files[0];
  }

  public upload() {
    alert(this.file.name);
    if(confirm("Are you sure to delete "+this.file.name)) {
      console.log("Implement delete functionality here");
    }
  }
}