import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { ClientMessage, MessageEdit } from 'src/app/shared/models/message';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForMessages } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MessagesService {
  baseUrl = environment.apiUrl;
  formData: MessageEdit = new MessageEdit();
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

  getMessages(userParams: UserParams) {
    let params = new HttpParams();
    params = params.append('sort', userParams.sort);
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForMessages>(this.baseUrl + 'messages', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getMessageById(id: number) {
    return this.http.get<ClientMessage>(this.baseUrl + 'messages/' + id);
  }

  updateMessage(formData){
    return this.http.put(this.baseUrl + 'messages/' + formData.id, formData);
  }
}









