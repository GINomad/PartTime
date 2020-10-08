import { Component, OnInit } from '@angular/core';
import { pipe } from 'rxjs';
import { ProfileService } from '../../core/profile/profile.service';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {

  constructor(private profileService: ProfileService) { }

  ngOnInit(): void {
  }

  call(){
    this.profileService.call().pipe().subscribe(
      result => {         
         if(result) {
           console.log(result);
         }
      });
  }
}
