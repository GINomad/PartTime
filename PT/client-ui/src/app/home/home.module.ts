import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common'; 
import { RouterModule } from '@angular/router';
import { IndexComponent } from './index/index.component';
import { HomeRoutingModule } from './home-routing.module';
import { AdModule } from '../ad/ad.module';

@NgModule({
  declarations: [IndexComponent],
  imports: [
    CommonModule,
    RouterModule,
    HomeRoutingModule,
    AdModule
  ]
})
export class HomeModule { }