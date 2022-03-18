import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Category, CategoryCreateEdit } from 'src/app/shared/models/category';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForCategories } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CategoriesService {
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

  getCategories(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForCategories>(this.baseUrl + 'categories', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getCategoryById(id: number) {
    return this.http.get<Category>(this.baseUrl + 'categories/' + id);
  }

  createCategory(categories: CategoryCreateEdit) {
    return this.http.post(this.baseUrl + 'categories', categories);
  }

  updateCategory(id: number, categories: CategoryCreateEdit) {
    return this.http.put(this.baseUrl + 'categories/' + id, categories);
  }

  deleteCategory(id: number) {
    return this.http.delete(this.baseUrl + 'categories/' + id);
  }
}

