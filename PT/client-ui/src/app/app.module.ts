import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { ConfigService } from './shared/config.service';
import { HttpClientModule } from '@angular/common/http';
import {A11yModule} from '@angular/cdk/a11y';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatToolbarModule} from '@angular/material/toolbar';
import { MatIconModule} from '@angular/material/icon';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations'

/* Module Imports */
import { CoreModule } from './core/core.module';
import { AccountModule }  from './account/account.module';
import { ShellModule } from './shell/shell.module';
import { SharedModule }   from './shared/shared.module';
import { HomeModule }   from './home/home.module';
import { ProfileModule }   from './profile/profile.module';
/* End Modules */
import { AuthGuard } from './core/authentication/auth.guard';
import { ProfileGuard } from './core/profile/profile.guard';


@NgModule({
  declarations: [
    AppComponent,
    AuthCallbackComponent
  ],
  imports: [
    A11yModule,
    BrowserAnimationsModule,
    MatToolbarModule,
    MatSidenavModule,
   MatListModule,
    MatIconModule,
    BrowserModule,  
    HttpClientModule,  
    CoreModule,
    HomeModule,     
    AccountModule,
    ProfileModule,    
    AppRoutingModule,
    ShellModule,   
    SharedModule
    
  ],
  providers: [
    ConfigService,
    AuthGuard,
    ProfileGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
