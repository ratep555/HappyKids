import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Category } from 'src/app/shared/models/category';
import { Discount, DiscountCreateEdit, DiscountEditClass, DiscountPutGet } from 'src/app/shared/models/discount';
import { Manufacturer } from 'src/app/shared/models/manufacturer';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForDiscounts } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DiscountsService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  formData: DiscountEditClass = new DiscountEditClass();

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

  getAllDiscounts(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForDiscounts>(this.baseUrl + 'discounts', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getDiscountById(id: number) {
    return this.http.get<Discount>(this.baseUrl + 'discounts/' + id);
  }

  createDiscount(discount: DiscountCreateEdit) {
    return this.http.post(this.baseUrl + 'discounts', discount);
  }

  updateDiscount(formData){
    return this.http.put(this.baseUrl + 'discounts/' + formData.id, formData);
  }

  putGetDiscount(id: number): Observable<DiscountPutGet>{
    return this.http.get<DiscountPutGet>(this.baseUrl + 'discounts/putget/' + id);
  }

  getCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'discounts/categories');
  }

  getManufacturers() {
    return this.http.get<Manufacturer[]>(this.baseUrl + 'discounts/manufacturers');
  }

  deleteDiscount(id: number) {
    return this.http.delete(this.baseUrl + 'discounts/' + id);
  }

}
