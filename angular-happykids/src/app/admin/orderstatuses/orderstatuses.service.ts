import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { UserParams } from 'src/app/shared/models/myparams';
import { OrderStatus, OrderStatusCreateEdit } from 'src/app/shared/models/orderStatus';
import { IPaginationForOrderStatuses } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderstatusesService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;

  constructor(private http: HttpClient,
              private accountService: AccountService) {
    this.accountService.currentUser$.pipe(take (1)).subscribe(user => {
      this.user = user;
      this.userParams = new UserParams(user);
});
}

  getUserParams() {
    return this.userParams;
  }

  setUserParams(params: UserParams) {
    this.userParams = params;
  }

  resetUserParams() {
    this.userParams = new UserParams(this.user);
    return this.userParams;
  }

  getOrderStatuses(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForOrderStatuses>(this.baseUrl + 'orderStatuses', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getOrderStatusById(id: number) {
    return this.http.get<OrderStatus>(this.baseUrl + 'orderStatuses/' + id);
  }

  createOrderStatus(orderstatus: OrderStatusCreateEdit) {
    return this.http.post(this.baseUrl + 'orderStatuses', orderstatus);
  }

  updateOrderStatus(id: number, orderstatus: OrderStatusCreateEdit) {
    return this.http.put(this.baseUrl + 'orderStatuses/' + id, orderstatus);
  }

  deleteOrderStatus(id: number) {
    return this.http.delete(this.baseUrl + 'orderStatuses/' + id);
  }
}


