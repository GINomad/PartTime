import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule }   from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { SharedModule }   from '../shared/shared.module';
import { MatFormFieldModule} from '@angular/material/form-field';
import { MatInputModule} from '@angular/material/input';

import { AccountRoutingModule } from './account.routing-module';
import { AuthService }  from '../core/authentication/auth.service';

@NgModule({
  schemas:[CUSTOM_ELEMENTS_SCHEMA],
  declarations: [LoginComponent, RegisterComponent],
  providers: [AuthService],
  imports: [
    CommonModule,
    FormsModule,
    AccountRoutingModule,
    SharedModule,
    MatInputModule,
    MatFormFieldModule  
  ]
})
export class AccountModule { }
