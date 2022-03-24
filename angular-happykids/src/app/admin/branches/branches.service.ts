import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map, take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { Branch, BranchCreateEdit } from 'src/app/shared/models/branch';
import { UserParams } from 'src/app/shared/models/myparams';
import { IPaginationForBranches } from 'src/app/shared/models/pagination';
import { User } from 'src/app/shared/models/user';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BranchesService {
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

  getAllBranches(userParams: UserParams) {
    let params = new HttpParams();
    if (userParams.query) {
      params = params.append('query', userParams.query);
    }
    params = params.append('page', userParams.page.toString());
    params = params.append('pageCount', userParams.pageCount.toString());
    return this.http.get<IPaginationForBranches>(this.baseUrl + 'branches', {observe: 'response', params})
    .pipe(
      map(response  => {
        return response.body;
      })
    );
  }

  getBranchById(id: number) {
    return this.http.get<Branch>(this.baseUrl + 'branches/' + id);
  }

  createBranch(branch: BranchCreateEdit) {
    return this.http.post(this.baseUrl + 'branches', branch);
  }

  updateBranch(id: number, branch: BranchCreateEdit){
    return this.http.put(this.baseUrl + 'branches/' + id, branch);
  }

  deleteBranch(id: number) {
    return this.http.delete(this.baseUrl + 'branches/' + id);
  }

}

