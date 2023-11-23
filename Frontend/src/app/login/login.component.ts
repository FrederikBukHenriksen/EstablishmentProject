import { HttpErrorResponse } from '@angular/common/http';
import {
  Component,
  EventEmitter,
  Inject,
  OnInit,
  Output,
  inject,
} from '@angular/core';
import { loginFormDef } from './login.model';
import { LoginService } from '../services/login-service/login-service.service';
import { Router } from '@angular/router';
// import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  /**
   *
   */
  constructor(private loginService: LoginService, private router: Router) {}

  loginFormDef = loginFormDef;

  ngOnInit(): void {
    this.handleLogin();
  }

  protected onLogin() {
    console.log('onLogin');
    this.loginService.Login(
      this.loginFormDef.value.name!,
      this.loginFormDef.value.password!
    );
  }

  protected handleLogin() {
    this.loginService.loginSuccessful.subscribe({
      error: (error: Error) => {
        console.log('not signed in, login-comp.', error.message);
      },
      next: () => {
        console.log('login successful recived');

        this.router.navigate(['/select-establishment']);
      },
    });
  }
}
