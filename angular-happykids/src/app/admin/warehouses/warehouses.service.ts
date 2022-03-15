import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Country } from 'src/app/shared/models/country';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForWarehouses } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { Warehouse, WarehouseCreateEdit } from 'src/app/shared/models/warehouse';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class WarehousesService {
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

  getWarehouses(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForWarehouses>(this.baseUrl + 'warehouses', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getWarehouseById(id: number) {
    return this.http.get<Warehouse>(this.baseUrl + 'warehouses/' + id);
  }

  createWarehouse(warehouse: WarehouseCreateEdit) {
    return this.http.post(this.baseUrl + 'warehouses', warehouse);
  }

  updateWarehouse(id: number, warehouse: WarehouseCreateEdit) {
    return this.http.put(this.baseUrl + 'warehouses/' + id, warehouse);
  }

  deleteWarehouse(id: number) {
    return this.http.delete(this.baseUrl + 'warehouses/' + id);
  }

  getCountries() {
    return this.http.get<Country[]>(this.baseUrl + 'warehouses/countries');
  }

}
