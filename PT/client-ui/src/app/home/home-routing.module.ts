import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { IndexComponent } from './index/index.component';
import { Shell } from '../shell/shell.service';
import { AuthGuard } from '../core/authentication/auth.guard';
import { ProfileGuard } from '../core/profile/profile.guard';

const routes: Routes = [
  Shell.childRoutes([
    { path: '', redirectTo: '/home', pathMatch: 'full', canActivate: [AuthGuard, ProfileGuard]},
    { path: 'home', component: IndexComponent, canActivate: [AuthGuard, ProfileGuard]}
  ])
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
  providers: []
})
export class HomeRoutingModule { }