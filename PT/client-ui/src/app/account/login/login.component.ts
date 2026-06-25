import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { finalize } from 'rxjs/operators';
import { AuthService } from '../../core/authentication/auth.service';
import { UserLogin } from '../../shared/models/user.login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit { 
  error: string | null;
  hide = true;
  userLogin: UserLogin = new UserLogin('', '', false, '/home');

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private spinner: NgxSpinnerService) { }

  ngOnInit() {
    this.userLogin.returnUrl = this.route.snapshot.queryParams.redirect || '/home';
  }

  onSubmit() {
    this.spinner.show();
    this.error = null;

    this.authService.login(this.userLogin)
      .pipe(finalize(() => this.spinner.hide()))
      .subscribe(
        () => this.router.navigateByUrl(this.userLogin.returnUrl),
        error => this.error = error);
  }
}
