import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ProfileSetupComponent } from './profile-setup/profile-setup.component';
import { ProfileComponent } from './profile/profile.component';
import { Shell } from '../shell/shell.service';
import { AuthGuard } from '../core/authentication/auth.guard';
import { ProfileGuard } from '../core/profile/profile.guard';

const routes: Routes = [
  Shell.childRoutes([
    { path: 'profile-setup', component: ProfileSetupComponent, canActivate: [AuthGuard]},
    { path: 'profile', component: ProfileComponent, canActivate: [AuthGuard, ProfileGuard]}
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class ProfileRoutingModule { }