import { Component, Injectable, OnInit, Inject } from '@angular/core';
import { AuthService } from './../auth/auth.service';
import { FilesService } from './files.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { FilesFilterService } from './files-filter.service';
import { ConfirmationDialogService } from '../confirmation-dialog/confirmation-dialog.service';
import { Router } from '@angular/router';
import { saveAs } from 'file-saver';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { RenameDialogComponent } from '../rename-dialog/rename-dialog.component';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

  constructor(public auth: AuthService, public upload: Upload, public confirm: ConfirmationDialogService, public filter: FilesFilterService,
    public dialog: MatDialog, private service: FilesService, private router: Router) { } // , private route: ActivatedRoute
  
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

      this.service.delete(fileName)
      .subscribe(() => {
        this.getFiles();
      },
      (errorResponse) => {
        
        console.error(errorResponse.error['error']);
      });
    }
  }

  public async rename(fileName: string) {
    
    let newName: string;

    const dialogRef = this.dialog.open(RenameDialog, {
      width: '67%',
      
      data: { name: fileName, new_name: fileName }
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('New name: ' + result);
      //this.animal = result;
    });
  }

  public download(fileName: string) {

    this.service.download(fileName)
    .subscribe(x => {

      const blob:any = new Blob([x], { type: "application/octet-stream" });
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

  public uploadFile(type) {
    
    console.log(type);
    
    if (type === 'add') {

      this.service.upload(this.upload.file).subscribe(() => {
        this.getFiles();
      },
      (errorResponse) => {

        console.error(errorResponse.error['error']);
      });
    }
    else if (type === 'edit') {
      
      this.service.upload(this.upload.file).subscribe(() => {
        this.getFiles();
      },
      (errorResponse) => {

        console.error(errorResponse.error['error']);
      });
    }
  }
}

export interface RenameDialogData {
  name: string;
  new_name: string;
}

@Component({
  selector: 'rename-dialog',
  templateUrl: 'rename-dialog.html',
})
export class RenameDialog {

  constructor(
    public dialogRef: MatDialogRef<RenameDialog>,
    @Inject(MAT_DIALOG_DATA) public data: RenameDialogData) {} // , private component: FilesComponent

  onNoClick(): void {
    this.dialogRef.close();
  }

  onClick(): void {
    alert("Beep");
    this.dialogRef.close();
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