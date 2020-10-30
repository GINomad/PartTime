import { Component, OnInit } from '@angular/core';
import {DomSanitizer} from '@angular/platform-browser';
import {MatIconRegistry} from '@angular/material/icon';
import { ProfileService } from 'src/app/core/profile/profile.service';
import { AuthService } from 'src/app/core/authentication/auth.service';

@Component({
  selector: 'profile-card',
  templateUrl: './profile-card.component.html',
  styleUrls: ['./profile-card.component.scss']
})
export class ProfileCardComponent implements OnInit {

  firstName: string = ""
  lastName: string = ""
  constructor(private profileService: ProfileService, private authService: AuthService, iconRegistry: MatIconRegistry, sanitizer: DomSanitizer) { 
    iconRegistry.addSvgIcon(
      'account-profile',
      sanitizer.bypassSecurityTrustResourceUrl('assets/account_circle.svg'));
  }

  ngOnInit(): void {
    this.profileService.getProfile(this.authService.profile.clientId).subscribe(client => {
      this.firstName = client.firstName;
      this.lastName = client.lastName;
    })
  }

}
