import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileSetupComponent } from './profile-setup/profile-setup.component';
import { ProfileComponent } from './profile/profile.component';
import { RouterModule } from '@angular/router';
import { ProfileRoutingModule } from './profile.routing-module'



@NgModule({
  declarations: [ProfileSetupComponent, ProfileComponent],
  imports: [
    CommonModule,
    RouterModule,
    ProfileRoutingModule
  ]
})
export class ProfileModule { }
