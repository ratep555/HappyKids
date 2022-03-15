import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';
import { ChildrenItemWarehouseCreateEdit } from 'src/app/shared/models/childrenItemWarehouse';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForChildrenItemWarehouses } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { Warehouse } from 'src/app/shared/models/warehouse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChildrenitemWarehousesService {
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

  getChildrenItemWarehouses(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForChildrenItemWarehouses>(this.baseUrl + 'childrenItemWarehouses', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  createChildrenItemWarehouse(childrenitemwarehouse: ChildrenItemWarehouseCreateEdit) {
    return this.http.post(this.baseUrl + 'childrenItemWarehouses', childrenitemwarehouse);
  }

  updateChildrenItemWarehouse(id: number, warehouseid: number, params: any) {
    return this.http.put(this.baseUrl + 'childrenItemWarehouses/' + id + '/' + warehouseid , params);
  }

  getChildrenItems() {
    return this.http.get<ChildrenItem[]>(this.baseUrl + 'childrenItemWarehouses/childrenitems');
  }

  getWarehouses() {
    return this.http.get<Warehouse[]>(this.baseUrl + 'childrenItemWarehouses/warehouses');
  }

  getChildrenItemWarehouseByChildrenItemIdAndWarehouseId(id: number, warehouseid: number) {
    return this.http.get(this.baseUrl + 'childrenItemWarehouses/' + id + '/' + warehouseid);
  }

}
