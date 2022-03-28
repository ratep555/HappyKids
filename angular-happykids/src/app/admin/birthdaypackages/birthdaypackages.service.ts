import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BirthdayPackage, BirthdayPackageCreateEdit, BirthdayPackagePutGet } from 'src/app/shared/models/birthdaypackage';
import { KidActivity } from 'src/app/shared/models/kidactivity';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForBirthdayPackages } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BirthdaypackagesService {
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

  getBirthdayPackages(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForBirthdayPackages>(this.baseUrl + 'birthdayPackages', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getBirthdayPackageById(id: number) {
    return this.http.get<BirthdayPackage>(this.baseUrl + 'birthdayPackages/' + id);
  }

  putGetBirthdayPackage(id: number): Observable<BirthdayPackagePutGet>{
    return this.http.get<BirthdayPackagePutGet>(this.baseUrl + 'birthdayPackages/putget/' + id);
  }

  createBirthdayPackage(birthdayPackage: BirthdayPackageCreateEdit) {
    const formData = this.CreateFormData(birthdayPackage);
    return this.http.post(this.baseUrl + 'birthdayPackages', formData);
  }

  updateBirthdayPackage(id: number, birthdayPackage: BirthdayPackageCreateEdit){
    const formData = this.CreateFormData(birthdayPackage);
    return this.http.put(this.baseUrl + 'birthdayPackages/' + id, formData);
  }

  deleteBirthdayPackage(id: number) {
    return this.http.delete(this.baseUrl + 'birthdayPackages/' + id);
  }

  getAllKidActivities() {
    return this.http.get<KidActivity[]>(this.baseUrl + 'birthdayPackages/kidactivities');
  }

  private CreateFormData(birthdayPackage: BirthdayPackageCreateEdit): FormData {
    const formData = new FormData();
    formData.append('price', JSON.stringify(birthdayPackage.price));
    formData.append('numberOfParticipants', JSON.stringify(birthdayPackage.numberOfParticipants));
    formData.append('additionalBillingPerParticipant', JSON.stringify(birthdayPackage.additionalBillingPerParticipant));
    formData.append('duration', JSON.stringify(birthdayPackage.duration));
    if (birthdayPackage.id) {
      formData.append('id', JSON.stringify(birthdayPackage.id));
    }
    if (birthdayPackage.packageName){
    formData.append('packageName', birthdayPackage.packageName);
    }
    if (birthdayPackage.description){
    formData.append('description', birthdayPackage.description);
    }
    if (birthdayPackage.picture){
      formData.append('picture', birthdayPackage.picture);
    }
    if (birthdayPackage.discountsIds) {
      formData.append('discountsIds', JSON.stringify(birthdayPackage.discountsIds));
    }
    if (birthdayPackage.kidActivitiesIds) {
      formData.append('kidActivitiesIds', JSON.stringify(birthdayPackage.kidActivitiesIds));
    }
    return formData;
  }

}


