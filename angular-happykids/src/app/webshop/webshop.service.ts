import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Category } from '../shared/models/category';
import { ChildrenItem } from '../shared/models/childrenitem';
import { Manufacturer } from '../shared/models/manufacturer';
import { MyParams } from '../shared/models/myparams';
import { IPaginationForChildrenItems, PaginationForChildrenItems } from '../shared/models/pagination';
import { Tag } from '../shared/models/tag';

@Injectable({
  providedIn: 'root'
})
export class WebshopService {
  baseUrl = environment.apiUrl;
  childrenItems: ChildrenItem[] = [];
  pagination = new PaginationForChildrenItems();
  myParams = new MyParams();
  tags: Tag[] = [];
  categories: Category[] = [];
  manufacturers: Manufacturer[] = [];

  constructor(private http: HttpClient) { }

  setMyParams(params: MyParams) {
    this.myParams = params;
  }

  getMyParams() {
    return this.myParams;
  }

  getChildrenItems() {
    let params = new HttpParams();
    if (this.myParams.manufacturerId !== 0) {
      params = params.append('manufacturerId', this.myParams.manufacturerId.toString());
    }
    if (this.myParams.tagId !== 0) {
      params = params.append('tagId', this.myParams.tagId.toString());
    }
    if (this.myParams.categoryId !== 0) {
      params = params.append('categoryId', this.myParams.categoryId.toString());
    }
    if (this.myParams.query) {
      params = params.append('query', this.myParams.query);
    }
    params = params.append('page', this.myParams.page.toString());
    params = params.append('pageCount', this.myParams.pageCount.toString());
    return this.http.get<IPaginationForChildrenItems>(this.baseUrl + 'childrenItems', { observe: 'response', params })
      .pipe(
        map(response => {
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  getCategoriesAssociatedWithChildrenItems() {
    return this.http.get<Category[]>(this.baseUrl + 'childrenItems/categoriesassociatedwithchildrenitems');
  }

  getManufacturersAssociatedWithChildrenItems() {
    return this.http.get<Manufacturer[]>(this.baseUrl + 'childrenItems/manufacturersassociatedwithchildrenitems');
  }

  getTagsAssociatedWithChildrenItems() {
    return this.http.get<Tag[]>(this.baseUrl + 'childrenItems/tagsassociatedwithchildrenitems');
  }

}











