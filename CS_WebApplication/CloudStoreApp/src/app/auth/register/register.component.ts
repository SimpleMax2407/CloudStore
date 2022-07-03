import { Component, OnInit } from '@angular/core';
import { AuthService } from './../auth.service';
import { Router } from '@angular/router';
import { HttpResponse } from '@angular/common/http';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  formData: any = {};
  errors: any = [];

  constructor(private auth: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  register(): void {
    alert("Start");
    this.errors = [];
    this.auth.register(this.formData)
      .subscribe(() => {
        this.router.navigate(['/auth/login'], { queryParams: { registered: 'success' } });
       },
        (errorResponse) => {
          if (errorResponse.status === 0) {
            this.errors.push("Server is not available");
          }
          else {
          this.errors.push(errorResponse.error['error']);
          }
        });
  }
}