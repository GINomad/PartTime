import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdComponent } from './ad/ad.component';
import { MyAddsComponent } from './my-adds/my-adds.component';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule} from '@angular/material/icon';



@NgModule({
  declarations: [AdComponent, MyAddsComponent],
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule
  ],
  exports: [AdComponent]
})
export class AdModule { }
