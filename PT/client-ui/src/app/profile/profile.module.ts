import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProfileSetupComponent } from './profile-setup/profile-setup.component';
import { ProfileComponent } from './profile/profile.component';
import { RouterModule } from '@angular/router';
import { ProfileRoutingModule } from './profile.routing-module';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule} from '@angular/material/form-field';
import { MatInputModule} from '@angular/material/input';
import { MatDatepickerModule } from '@angular/material/datepicker';
import {MatNativeDateModule} from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { ReactiveFormsModule, FormsModule} from '@angular/forms';



@NgModule({
  declarations: [ProfileSetupComponent, ProfileComponent],
  imports: [
    CommonModule,
    RouterModule,
    ProfileRoutingModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatDatepickerModule,
    MatNativeDateModule,
    MatButtonModule,
    ReactiveFormsModule,
    FormsModule
  ]
})
export class ProfileModule { }
