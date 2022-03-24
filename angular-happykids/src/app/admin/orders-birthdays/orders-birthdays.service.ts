import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BirthdayOrderEdit, ClientBirthdayOrder } from 'src/app/shared/models/birthdayorder';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForClientBirthayOrders } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrdersBirthdaysService {
  baseUrl = environment.apiUrl;
  user: User;
  userParams: UserParams;
  formData: BirthdayOrderEdit = new BirthdayOrderEdit();

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

  getAllBirthdayOrders(userParams: UserParams) {
    let params = new HttpParams();
    params = params.append('sort', userParams.sort);
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForClientBirthayOrders>(this.baseUrl + 'birthdayOrders', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getBirthdayOrderById(id: number) {
    return this.http.get<ClientBirthdayOrder>(this.baseUrl + 'birthdayOrders/' + id);
  }

  updateBirthdayOrder(formData){
    return this.http.put(this.baseUrl + 'birthdayOrders/' + formData.id, formData);
  }

}











