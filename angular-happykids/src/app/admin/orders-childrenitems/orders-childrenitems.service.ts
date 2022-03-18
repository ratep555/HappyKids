import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { UserParams } from 'src/app/shared/models/myparams';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { IPaginationForChildrenItemsOrders } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdersChildrenitemsService {
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

  getAllOrdersForChildrenItems(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.orderStatusId !== 0) {
      params = params.append('orderStatusId', userParams.orderStatusId.toString());
    }
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForChildrenItemsOrders>(this.baseUrl + 'orders', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getOrderById(id: number) {
    return this.http.get(this.baseUrl + 'orders/' + id);
  }

  updateOrder(id: number, params: any) {
    return this.http.put(this.baseUrl + 'orders/' + id, params);
  }

  getOrderStatuses() {
    return this.http.get<OrderStatus[]>(this.baseUrl + 'orders/orderstatuses');
  }

  getOrderStatusesAssociatedWithOrdersForChildrenItems() {
    return this.http.get<OrderStatus[]>(this.baseUrl + 'orders/orderstatusesforfiltering');
  }

}
