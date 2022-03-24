import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { BirthdayOrderCreate } from '../shared/models/birthdayorder';
import { BirthdayPackage } from '../shared/models/birthdaypackage';
import { MyParams } from '../shared/models/myparams';
import { IPaginationForBirthdayPackages, PaginationForBirthdayPackages } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class BirthdaysService {
  baseUrl = environment.apiUrl;
  birthdayPackages: BirthdayPackage[] = [];
  pagination = new PaginationForBirthdayPackages();
  myParams = new MyParams();

  constructor(private http: HttpClient) { }

  setMyParams(params: MyParams) {
    this.myParams = params;
  }

  getMyParams() {
    return this.myParams;
  }

  getBirthdayPackages() {
    let params = new HttpParams();
    if (this.myParams.query) {
      params = params.append('query', this.myParams.query);
    }
    params = params.append('page', this.myParams.page.toString());
    params = params.append('pageCount', this.myParams.pageCount.toString());
    return this.http.get<IPaginationForBirthdayPackages>
    (this.baseUrl + 'birthdayPackages', { observe: 'response', params })
      .pipe(
        map(response => {
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  getPureBirthdayPackages() {
    return this.http.get<BirthdayPackage[]>(this.baseUrl + 'birthdayOrders/pure');
  }

  createBirthdayOrder(birthday: BirthdayOrderCreate) {
    return this.http.post(this.baseUrl + 'birthdayOrders', birthday);
  }


}
