import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthCallbackComponent } from './auth-callback/auth-callback.component';
import { ConfigService } from './shared/config.service';
import { HttpClientModule } from '@angular/common/http';

/* Module Imports */
import { CoreModule } from './core/core.module';
import { AccountModule }  from './account/account.module';
import { ShellModule } from './shell/shell.module';
import { SharedModule }   from './shared/shared.module';
import { HomeModule }   from './home/home.module';


@NgModule({
  declarations: [
    AppComponent,
    AuthCallbackComponent
  ],
  imports: [
    BrowserModule,  
    HttpClientModule, 
    CoreModule,
    AccountModule,  
    AppRoutingModule,
    ShellModule,   
    SharedModule,
    HomeModule 
  ],
  providers: [
    ConfigService,
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
