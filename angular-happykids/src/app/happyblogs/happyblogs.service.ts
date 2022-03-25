import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Blog } from '../shared/models/blog';
import { BlogCommentClass, BlogCommentCreateEditClass } from '../shared/models/blogcomment';
import { MyParams } from '../shared/models/myparams';
import { IPaginationForBlogs, PaginationForBlogs } from '../shared/models/pagination';

@Injectable({
  providedIn: 'root'
})
export class HappyblogsService {
  baseUrl = environment.apiUrl;
  pagination = new PaginationForBlogs();
  myParams = new MyParams();

  constructor(private http: HttpClient) { }

  setMyParams(params: MyParams) {
    this.myParams = params;
  }

  getMyParams() {
    return this.myParams;
  }

  getAllBlogs() {
    let params = new HttpParams();
    if (this.myParams.query) {
      params = params.append('query', this.myParams.query);
    }
    params = params.append('page', this.myParams.page.toString());
    params = params.append('pageCount', this.myParams.pageCount.toString());
    return this.http.get<IPaginationForBlogs>
    (this.baseUrl + 'blogs', { observe: 'response', params })
      .pipe(
        map(response => {
          this.pagination = response.body;
          return this.pagination;
        })
      );
  }

  upsertBlogComment(model: BlogCommentCreateEditClass): Observable<BlogCommentClass>{
    return this.http.post<BlogCommentClass>(this.baseUrl + 'blogs/blogcomments', model);
  }

  getAllBlogComments(blogId: number): Observable<BlogCommentClass[]> {
    return this.http.get<BlogCommentClass[]>(this.baseUrl + 'blogs/blogcomments/' + blogId);
  }

  deleteBlogComment(id: number): Observable<number>  {
    return this.http.delete<number>(this.baseUrl + 'blogs/blogcomments/' + id);
  }

}
