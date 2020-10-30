import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { AuthService } from 'src/app/core/authentication/auth.service';

@Component({
  selector: 'app-shell',
  templateUrl: './shell.component.html',
  styleUrls: ['./shell.component.scss']
})
export class ShellComponent implements OnInit, OnDestroy {

  isAuthenticated: boolean;
  hasClientId: boolean;
  subscription:Subscription;
  clientIdSubscription: Subscription;
  constructor(private authService:AuthService) { }

  ngOnInit() {
    this.subscription = this.authService.authNavStatus$.subscribe(status => this.isAuthenticated = status);
    this.clientIdSubscription = this.authService.clientIdPresenceStatus$.subscribe(status => this.hasClientId = status);
  } 

  ngOnDestroy() {
    // prevent memory leak when component is destroyed
    this.subscription.unsubscribe();
    this.clientIdSubscription.unsubscribe();
  }

}
