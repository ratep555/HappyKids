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
  private currentUserSource = new ReplaySubject<User>(1);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient,
              private router: Router,
              private externalAuthService: SocialAuthService) { }

  register(model: any) {
    return this.http.post(this.baseUrl + 'account/register', model).pipe(
      map((user: User) => {
        if (user) {
          this.setCurrentUser(user);
        }
      })
    );
  }

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

  setCurrentUser(user: User) {
    user.roles = [];
    const roles = this.getDecodedToken(user.token).role;
    Array.isArray(roles) ? user.roles = roles : user.roles.push(roles);
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

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









