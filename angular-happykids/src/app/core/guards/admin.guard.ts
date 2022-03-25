import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { User } from 'src/app/shared/models/user';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private accountService: AccountService,
              private router: Router,
              private toastr: ToastrService) {}

canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
let currentUser: User;
this.accountService.currentUser$.pipe(take(1)).subscribe(user => currentUser = user);
if (currentUser?.roles.includes('Admin')) {
return true;
}
this.router.navigate(['**']);
return false;
}
}

