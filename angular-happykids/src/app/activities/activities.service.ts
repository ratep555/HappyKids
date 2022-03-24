import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { KidActivity } from '../shared/models/kidactivity';
import { MyParams } from '../shared/models/myparams';
import { PaginationForKidActivities } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class ActivitiesService {
  baseUrl = environment.apiUrl;
  kidActivities: KidActivity[] = [];
  pagination = new PaginationForKidActivities();
  myParams = new MyParams();

  constructor(private http: HttpClient) { }

  setMyParams(params: MyParams) {
    this.myParams = params;
  }

  getMyParams() {
    return this.myParams;
  }

  getKidActivities() {
    let params = new HttpParams();
    if (this.myParams.query) {
      params = params.append('query', this.myParams.query);
    }
    params = params.append('sort', this.myParams.sort);
    params = params.append('page', this.myParams.page.toString());
    params = params.append('pageCount', this.myParams.pageCount.toString());
    return this.http.get<PaginationForKidActivities>
    (this.baseUrl + 'kidActivities', { observe: 'response', params })
      .pipe(
        map(response => {
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }
}
