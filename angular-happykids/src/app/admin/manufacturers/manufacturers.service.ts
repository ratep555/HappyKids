import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Manufacturer, ManufacturerCreateEdit } from 'src/app/shared/models/manufacturer';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForManufacturers } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ManufacturersService {
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

  getManufacturers(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForManufacturers>(this.baseUrl + 'manufacturers', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getManufacturerById(id: number) {
    return this.http.get<Manufacturer>(this.baseUrl + 'manufacturers/' + id);
  }

  createManufacturer(manufacturer: ManufacturerCreateEdit) {
    return this.http.post(this.baseUrl + 'manufacturers', manufacturer);
  }

  updateManufacturer(id: number, manufacturer: ManufacturerCreateEdit) {
    return this.http.put(this.baseUrl + 'manufacturers/' + id, manufacturer);
  }

  deleteManufacturer(id: number) {
    return this.http.delete(this.baseUrl + 'manufacturers/' + id);
  }
}
