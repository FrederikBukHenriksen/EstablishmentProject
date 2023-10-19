import { TestBed, fakeAsync, tick } from '@angular/core/testing';
import { AuthenticationClient, Role, User } from 'api';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { LoginService } from './login-service.service';
import { of } from 'rxjs/internal/observable/of';
import { Observable } from 'rxjs/internal/Observable';
import { throwError } from 'rxjs/internal/observable/throwError';

describe('LoginService', () => {
  let loginService: LoginService;
  let authenticationClient: jasmine.SpyObj<AuthenticationClient>;

  beforeEach(() => {
    authenticationClient = jasmine.createSpyObj('AuthenticationClient', [
      'logIn',
      'logOut',
      'getLoggedInUser',
      'isLoggedIn',
    ]);

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        {
          provide: AuthenticationClient,
          useValue: authenticationClient,
        },
      ],
    });
    loginService = TestBed.inject(LoginService);
    authenticationClient = TestBed.inject(
      AuthenticationClient
    ) as jasmine.SpyObj<AuthenticationClient>;
  });

  describe('Test login functionality', () => {
    it('User signed in succesfully', () => {
      let loginSuccessful = false;
      loginService.loginSuccessful.subscribe(() => {
        loginSuccessful = true;
      });

      authenticationClient.logIn.and.returnValue(of() as Observable<void>);

      loginService.Login('frederik', 'password1234');

      expect(loginSuccessful).toBe(true);
    });

    it('User signed in unsuccesfully', () => {
      let loginSuccessful = false;
      loginService.loginSuccessful.subscribe(() => {
        loginSuccessful = true;
      });

      authenticationClient.logIn.and.returnValue(throwError(new Error()));

      loginService.Login('frederik', 'password1234');

      expect(loginSuccessful).toBe(false);
    });
  });

  describe('Test getUser functionality', () => {
    const user = {
      username: 'frederik',
      password: 'password1234',
      role: Role.Admin,
    } as User;

    it('User retrieving successfully', async () => {
      authenticationClient.getLoggedInUser.and.returnValue(
        of(user) as Observable<User>
      );

      const retrievedUser = await loginService.GetLoggedInUser();

      expect(retrievedUser).toEqual(user);
    });

    it('User retrieving unsuccessfully', async () => {
      authenticationClient.getLoggedInUser.and.returnValue(
        throwError(new Error())
      );

      const retrievedUser = await loginService.GetLoggedInUser();

      expect(retrievedUser).toBeUndefined();
    });
  });

  describe('Test isLoggedIn functionality', () => {
    it('User is logged in', async () => {
      authenticationClient.isLoggedIn.and.returnValue(of(true));

      const isLoggedIn = await loginService.IsLoggedIn();

      expect(isLoggedIn).toBe(true);
    });

    it('User is not logged in', async () => {
      authenticationClient.isLoggedIn.and.returnValue(of(false));

      const isLoggedIn = await loginService.IsLoggedIn();

      expect(isLoggedIn).toBe(false);
    });
  });
});
