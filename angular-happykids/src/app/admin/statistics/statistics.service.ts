import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { Statistics } from 'src/app/shared/models/statistics';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StatisticsService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCountForEntities() {
    return this.http.get<Statistics>(this.baseUrl + 'admin/statistics');
  }

  getNumberOfBuyersForEachPaymentOption() {
    return this.http.get<any>(this.baseUrl + 'admin/charts1').pipe(
    map( result => {
      console.log(result);
      return result;
    })
    );
  }

  getAllOrderStatusesForChildrenItems() {
    return this.http.get<any>(this.baseUrl + 'admin/charts2').pipe(
    map( result => {
      console.log(result);
      return result;
    })
    );
  }

  getAllOrderStatusesForBirthdayOrders() {
    return this.http.get<any>(this.baseUrl + 'admin/charts3').pipe(
    map( result => {
      console.log(result);
      return result;
    })
    );
  }

}




