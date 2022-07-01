import { Component, OnInit } from '@angular/core';
import { AuthService } from './../auth/auth.service';
import { FilesService } from './files.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrls: ['./files.component.css']
})
export class FilesComponent implements OnInit {

  constructor(public auth: AuthService, private service: FilesService, private router: Router) { }
  public files;

  ngOnInit(): void {
    if (!this.auth.isAuthenticated()) {
      this.router.navigate(["/auth/login"]);
      return;
    }
    
    this.getFiles();
    
  }

  public getFiles() {
    this.service.getAllFiles()
    .subscribe((files) => {
      return files;
     },
      (errorResponse) => {
        console.error(errorResponse.status)
        console.error(errorResponse.error['error']);
      });
  }

}
