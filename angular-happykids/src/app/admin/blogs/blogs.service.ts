import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Blog, BlogCreateEdit } from 'src/app/shared/models/blog';
import { BlogCommentClass, BlogCommentCreateEditClass } from 'src/app/shared/models/blogcomment';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForBlogs } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BlogsService {
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

  getAllBlogsForUser(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForBlogs>(this.baseUrl + 'blogs/user', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getBlogById(id: number) {
    return this.http.get<Blog>(this.baseUrl + 'blogs/' + id);
  }

  createBlog(blog: BlogCreateEdit) {
    const formData = this.CreateFormData(blog);
    return this.http.post(this.baseUrl + 'blogs', formData);
  }

  updateBlog(id: number, blog: BlogCreateEdit){
    const formData = this.CreateFormData(blog);
    return this.http.put(this.baseUrl + 'blogs/' + id, formData);
  }

  private CreateFormData(blog: BlogCreateEdit): FormData {
    const formData = new FormData();
    if (blog.id) {
    formData.append('id', JSON.stringify(blog.id));
    }
    if (blog.title){
    formData.append('title', blog.title);
    }
    if (blog.blogContent){
    formData.append('blogContent', blog.blogContent);
    }
    if (blog.picture){
      formData.append('picture', blog.picture);
    }
    return formData;
  }


}






