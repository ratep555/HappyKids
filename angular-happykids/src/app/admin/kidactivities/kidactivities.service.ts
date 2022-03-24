import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { KidActivity, KidActivityCreateEdit } from 'src/app/shared/models/kidactivity';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForKidActivities } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class KidactivitiesService {
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

  getAllKidActivities(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForKidActivities>(this.baseUrl + 'kidActivities', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getKidActivityById(id: number) {
    return this.http.get<KidActivity>(this.baseUrl + 'kidActivities/' + id);
  }

  createKidActivity(kidActivity: KidActivityCreateEdit) {
    const formData = this.CreateFormData(kidActivity);
    return this.http.post(this.baseUrl + 'kidActivities', formData);
  }

  updateKidActivity(id: number, kidActivity: KidActivityCreateEdit){
    const formData = this.CreateFormData(kidActivity);
    return this.http.put(this.baseUrl + 'kidActivities/' + id, formData);
  }

  private CreateFormData(kidActivity: KidActivityCreateEdit): FormData {
    const formData = new FormData();
    if (kidActivity.id) {
    formData.append('id', JSON.stringify(kidActivity.id));
    }
    if (kidActivity.name){
    formData.append('name', kidActivity.name);
    }
    if (kidActivity.description){
    formData.append('description', kidActivity.description);
    }
    if (kidActivity.picture){
      formData.append('picture', kidActivity.picture);
    }
    if (kidActivity.videoClip){
      formData.append('videoClip', kidActivity.videoClip);
    }
    return formData;
  }


}
