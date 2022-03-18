import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AccountService } from 'src/app/account/account.service';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket } from 'src/app/shared/models/basket';
import { User } from 'src/app/shared/models/user';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  currentUser$: Observable<User>;
  basket$: Observable<Basket>;
  isCollapsed = true;
  user: User;
  locationList = [];

  constructor(public accountService: AccountService,
              private basketService: BasketService,
              private router: Router) {}

 ngOnInit(): void {
 this.currentUser$ = this.accountService.currentUser$;
 this.basket$ = this.basketService.basket$;
}

logout() {
  this.accountService.logout();
}
}
