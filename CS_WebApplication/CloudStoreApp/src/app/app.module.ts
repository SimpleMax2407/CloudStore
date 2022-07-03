import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { MatCommonModule } from '@angular/material/core';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatFormFieldModule } from '@angular/material/form-field';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HeaderComponent } from './header/header.component';
import { HomeComponent } from './home/home.component';
import { ProfileComponent } from './profile/profile.component';
import { AuthComponent } from './auth/auth.component';
import { FormsModule, ReactiveFormsModule  } from '@angular/forms';

import { AuthModule } from './auth/auth.module';
import { FilesComponent } from './files/files.component';

import {
  ConfirmBoxConfigModule,
  DialogConfigModule,
  NgxAwesomePopupModule,
  ToastNotificationConfigModule,
} from '@costlydeveloper/ngx-awesome-popup';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

@NgModule({
  declarations: [
    AppComponent,
    HeaderComponent,
    HomeComponent,
    ProfileComponent,
    AuthComponent,
    FilesComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    AuthModule,
    FormsModule,
    ReactiveFormsModule,

    MatCommonModule,
    MatFormFieldModule,
    MatIconModule,
    MatMenuModule,

    NgxAwesomePopupModule.forRoot({
      colorList: {
        success: '#3caea3', // optional
        info: '#2f8ee5', // optional
        warning: '#ffc107', // optional
        danger: '#e46464', // optional
        customOne: '#3ebb1a', // optional
        customTwo: '#bd47fa', // optional (up to custom five)
      },
    }),
    ConfirmBoxConfigModule.forRoot(),

    DialogConfigModule.forRoot(), // optional
    ToastNotificationConfigModule.forRoot(), BrowserAnimationsModule, // optional
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }