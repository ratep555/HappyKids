import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Category } from 'src/app/shared/models/category';
import { ChildrenItem, ChildrenItemCreateEdit, ChildrenItemPutGet } from 'src/app/shared/models/childrenitem';
import { Discount } from 'src/app/shared/models/discount';
import { Manufacturer } from 'src/app/shared/models/manufacturer';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForChildrenItems } from 'src/app/shared/models/pagination';
import { Tag } from 'src/app/shared/models/tag';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ChildrenitemsService {
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

  getItems(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForChildrenItems>(this.baseUrl + 'childrenItems', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getChildrenItemById(id: number) {
    return this.http.get<ChildrenItem>(this.baseUrl + 'childrenItems/' + id);
  }

  createChildrenItem(childrenitem: ChildrenItemCreateEdit) {
    const formData = this.CreateFormData(childrenitem);
    return this.http.post(this.baseUrl + 'childrenItems', formData);
  }

  updateChildrenItem(id: number, item: ChildrenItemCreateEdit){
    const formData = this.CreateFormData(item);
    return this.http.put(this.baseUrl + 'childrenItems/' + id, formData);
  }

  putGetChildrenItem(id: number): Observable<ChildrenItemPutGet>{
    return this.http.get<ChildrenItemPutGet>(this.baseUrl + 'childrenItems/putget/' + id);
  }

  getAllCategories() {
    return this.http.get<Category[]>(this.baseUrl + 'childrenItems/categories');
  }

  getAllDiscounts() {
    return this.http.get<Discount[]>(this.baseUrl + 'childrenItems/discounts');
  }

  getAllManufacturers() {
    return this.http.get<Manufacturer[]>(this.baseUrl + 'childrenItems/manufacturers');
  }

  getAllTags() {
    return this.http.get<Tag[]>(this.baseUrl + 'childrenItems/tags');
  }

  private CreateFormData(childrenitem: ChildrenItemCreateEdit): FormData {
    const formData = new FormData();
    if (childrenitem.id) {
      formData.append('id', JSON.stringify(childrenitem.id));
    }
    formData.append('price', JSON.stringify(childrenitem.price));
    if (childrenitem.name){
    formData.append('name', childrenitem.name);
    }
    if (childrenitem.description){
    formData.append('description', childrenitem.description);
    }
    if (childrenitem.picture){
      formData.append('picture', childrenitem.picture);
    }
    if (childrenitem.categoriesIds) {
      formData.append('categoriesIds', JSON.stringify(childrenitem.categoriesIds));
    }
    if (childrenitem.discountsIds) {
      formData.append('discountsIds', JSON.stringify(childrenitem.discountsIds));
    }
    if (childrenitem.manufacturersIds) {
      formData.append('manufacturersIds', JSON.stringify(childrenitem.manufacturersIds));
    }
    if (childrenitem.tagsIds) {
      formData.append('tagsIds', JSON.stringify(childrenitem.tagsIds));
    }
    return formData;
  }

}










