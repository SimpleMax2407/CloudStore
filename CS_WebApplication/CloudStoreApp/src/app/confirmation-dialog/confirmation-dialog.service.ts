import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import {
  ConfirmBoxInitializer,
  DialogLayoutDisplay,
  DisappearanceAnimation,
  AppearanceAnimation
} from '@costlydeveloper/ngx-awesome-popup';

@Injectable({
  providedIn: 'root'
})
export class ConfirmationDialogService {
  
  ready = false;
  result = false;

  delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }

  public async getResult(): Promise<boolean> {
    while(!this.ready) {
      await new Promise(f => setTimeout(f, 1000));
    }

    return this.result;
  }

  public openConfirmDeletionBox() {
    const newConfirmBox = new ConfirmBoxInitializer();

    newConfirmBox.setTitle('Warning');
    newConfirmBox.setMessage('Are you sure?');

    // Choose layout color type
    newConfirmBox.setConfig({
    layoutType: DialogLayoutDisplay.WARNING, // SUCCESS | INFO | NONE | DANGER | WARNING
    animationIn: AppearanceAnimation.SWING, // BOUNCE_IN | SWING | ZOOM_IN | ZOOM_IN_ROTATE | ELASTIC | JELLO | FADE_IN | SLIDE_IN_UP | SLIDE_IN_DOWN | SLIDE_IN_LEFT | SLIDE_IN_RIGHT | NONE
    animationOut: DisappearanceAnimation.FLIP_OUT, // BOUNCE_OUT | ZOOM_OUT | ZOOM_OUT_WIND | ZOOM_OUT_ROTATE | FLIP_OUT | SLIDE_OUT_UP | SLIDE_OUT_DOWN | SLIDE_OUT_LEFT | SLIDE_OUT_RIGHT | NONE
    buttonPosition: 'right', // optional 
    });

    newConfirmBox.setButtonLabels('Delete', 'Cancel');
    
    this.ready = false;
    newConfirmBox.openConfirmBox$().subscribe(resp => {
      this.ready = true;
      this.result = resp.clickedButtonID == 'delete';
    });
``}

}
