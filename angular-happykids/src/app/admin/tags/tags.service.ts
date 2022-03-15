import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForTags } from 'src/app/shared/models/pagination';
import { Tag, TagCreateEdit } from 'src/app/shared/models/tag';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TagsService {
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

  getTags(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForTags>(this.baseUrl + 'tags', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getTagById(id: number) {
    return this.http.get<Tag>(this.baseUrl + 'tags/' + id);
  }

  createTag(tag: TagCreateEdit) {
    return this.http.post(this.baseUrl + 'tags', tag);
  }

  updateTag(id: number, tag: TagCreateEdit) {
    return this.http.put(this.baseUrl + 'tags/' + id, tag);
  }

  deleteTag(id: number) {
    return this.http.delete(this.baseUrl + 'tags/' + id);
  }
}

