import { Component, OnInit, Inject } from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';
import { FilesComponent, RenameDialogData } from '../files/files.component';

export interface DialogData {
  name: string;
  new_name: string;
}

@Component({
  selector: 'app-rename-dialog',
  templateUrl: './rename-dialog.component.html',
  styleUrls: ['./rename-dialog.component.css']
})
export class RenameDialogComponent {

  constructor(
    public dialogRef: MatDialogRef<RenameDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: RenameDialogData) {} // , private component: FilesComponent

  onNoClick(): void {
    this.dialogRef.close();
  }

  onClick(): void {
    alert("Beep");
    this.dialogRef.close();
  }
}
