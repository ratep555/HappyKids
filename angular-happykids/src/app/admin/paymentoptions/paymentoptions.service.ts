import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForPaymentOptions } from 'src/app/shared/models/pagination';
import { PaymentOption, PaymentOptionCreateEdit } from 'src/app/shared/models/paymentOption';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PaymentoptionsService {
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

  getPaymentOptions(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForPaymentOptions>(this.baseUrl + 'paymentOptions', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getPaymentOptionById(id: number) {
    return this.http.get<PaymentOption>(this.baseUrl + 'paymentOptions/' + id);
  }

  createPaymentOption(paymentoption: PaymentOptionCreateEdit) {
    return this.http.post(this.baseUrl + 'paymentOptions', paymentoption);
  }

  updatePaymentOption(id: number, paymentoption: PaymentOptionCreateEdit) {
    return this.http.put(this.baseUrl + 'paymentOptions/' + id, paymentoption);
  }

  deletePaymentOption(id: number) {
    return this.http.delete(this.baseUrl + 'paymentOptions/' + id);
  }
}
