import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthService } from '../authentication/auth.service';

@Injectable()
export class ProfileGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (this.authService.profile != null && this.authService.profile.client_user_id != null && this.authService.profile.client_user_id != "") { return true; }
    this.router.navigate(['/profile-setup'], { queryParams: { redirect: state.url }, replaceUrl: true });
    return false;
  }

}