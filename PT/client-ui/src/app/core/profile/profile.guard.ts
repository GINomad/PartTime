import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { AuthService } from '../authentication/auth.service';

@Injectable()
export class ProfileGuard implements CanActivate {

  constructor(private router: Router, private authService: AuthService) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    var hasClientId: boolean = false;
    this.authService.clientIdPresenceStatus$.subscribe(status => hasClientId = status)
    if (hasClientId) { return true; }
    this.router.navigate(['/profile-setup'], { queryParams: { redirect: state.url }, replaceUrl: true });
    return false;
  }

}