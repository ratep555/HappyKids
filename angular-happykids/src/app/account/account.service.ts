import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ReplaySubject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { User } from '../shared/models/user';
import { map } from 'rxjs/operators';
import { Address } from '../shared/models/address';
import { SocialAuthService } from 'angularx-social-login';
import { GoogleLoginProvider } from 'angularx-social-login';
import { ForgotPassword } from '../shared/models/forgotpassword';
import { ResetPassword } from '../shared/models/resetpassword';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  baseUrl = environment.apiUrl;
  // Replay subject does not emit initial value so our authguard will wait till this has some value
  private currentUserSource = new ReplaySubject<User>(1);
  // The reason we are setting this as observable is so that it can be observed by other components
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient,
              private router: Router,
              private externalAuthService: SocialAuthService) { }

  // Ads user - client
  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  // Adds manager
  addManager(model: any) {
    return this.http.post(this.baseUrl + 'account/addmanager', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  login(values: any) {
    return this.http.post(this.baseUrl + 'account/login', values)
     .pipe(
      map((response: User) => {
        const user = response;
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

  // We will use the information about roles in our guard
  // We are trying to see if the role in token is array or simple string
  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  // Google sign in
  public signInWithGoogle = () => {
    return this.externalAuthService.signIn(GoogleLoginProvider.PROVIDER_ID);
  }

  externalLogin(values: any) {
    return this.http.post(this.baseUrl + 'account/externallogin', values).pipe(
      map((user: User) => {
        if (user) {
          localStorage.setItem('user', JSON.stringify(user));
          this.currentUserSource.next(user);
        }
      })
    );
  }

  public signOutExternal = () => {
    this.externalAuthService.signOut();
  }

  logout() {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  forgotPassword(forgotpassword: ForgotPassword) {
    return this.http.post(this.baseUrl + 'account/forgotpassword', forgotpassword);
  }

  resetPassword(resetpassword: ResetPassword) {
    return this.http.post(this.baseUrl + 'account/resetpassword', resetpassword);
  }

  getDecodedToken(token) {
    return JSON.parse(atob(token.split('.')[1]));
  }

  getClientAddress() {
    return this.http.get<Address>(this.baseUrl + 'account/getaddress');
  }

  updateClientAddress(address: Address) {
    return this.http.put<Address>(this.baseUrl + 'account/updateaddress', address);
  }
  }









