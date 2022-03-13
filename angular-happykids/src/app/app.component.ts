import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { User } from './shared/models/user';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'angular-happykids';

constructor(private accountService: AccountService) {}

  ngOnInit() {
    this.loadCurrentUser();

   /*  const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.basketService.gettingBasket(basketId).subscribe(() => {
        console.log('initialised basket');
      }, error => {
        console.log(error);
      });
    } */
  }

  loadCurrentUser() {
    const user: User = JSON.parse(localStorage.getItem('user'));
    if (user) {
      this.accountService.setCurrentUser(user);
    }  }
}
