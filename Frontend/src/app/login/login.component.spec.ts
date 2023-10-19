import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoginComponent } from './login.component';
import {
  AuthenticationClient,
  Role,
  User,
  ApiException,
  LoginCommand,
} from 'api';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Observable, of, throwError } from 'rxjs';
import { HttpClient } from '@angular/common/http';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authenticationClient: jasmine.SpyObj<AuthenticationClient>;

  beforeEach(() => {
    authenticationClient = jasmine.createSpyObj('AuthenticationClient', [
      'login',
    ]); // Create a spy object for the service

    TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [HttpClientTestingModule, FormsModule, ReactiveFormsModule],
      providers: [
        {
          provide: AuthenticationClient,
          useValue: authenticationClient,
        },
      ],
    });

    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  const user = {
    username: 'frederik',
    password: 'password1234',
    role: Role.Admin,
  } as User;

  it('verify test woprks', () => {
    expect(true).toBe(true);
  });

  // it('backend accepts login credentials', async () => {}


  

  // it('backend rejects login credentials', async () => {
  //   let loginSuccess: boolean | undefined;
  //   component.loginSuccess.subscribe((value) => {
  //     loginSuccess = value;
  //   });

  //   const error = new ApiException('', 401, '', {}, undefined);
  //   authenticationClient.login.and.callFake((loginCommand: LoginCommand) => {
  //     return throwError(error);
  //   });

  //   component.onSubmit();

  //   expect(loginSuccess).toBe(false);
  // });
});
