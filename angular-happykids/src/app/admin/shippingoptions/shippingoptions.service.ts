import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForShippingOptions } from 'src/app/shared/models/pagination';
import { ShippingOption, ShippingOptionCreateEdit } from 'src/app/shared/models/shippingOption';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ShippingoptionsService {
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

  getShippingOptions(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForShippingOptions>(this.baseUrl + 'shippingOptions', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getShippingOptionById(id: number) {
    return this.http.get<ShippingOption>(this.baseUrl + 'shippingOptions/' + id);
  }

  createShippingOption(shippingoption: ShippingOptionCreateEdit) {
    return this.http.post(this.baseUrl + 'shippingOptions', shippingoption);
  }

  updateShippingOption(id: number, shippingoption: ShippingOptionCreateEdit) {
    return this.http.put(this.baseUrl + 'shippingOptions/' + id, shippingoption);
  }

  deleteShippingOption(id: number) {
    return this.http.delete(this.baseUrl + 'shippingOptions/' + id);
  }
}

