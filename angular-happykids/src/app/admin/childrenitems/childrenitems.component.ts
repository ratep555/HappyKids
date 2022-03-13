import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';
import { UserParams } from 'src/app/shared/models/myparams';
import { ChildrenitemsService } from './childrenitems.service';

@Component({
  selector: 'app-childrenitems',
  templateUrl: './childrenitems.component.html',
  styleUrls: ['./childrenitems.component.scss']
})
export class ChildrenitemsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  childrenitems: ChildrenItem[];
  userParams: UserParams;
  totalCount: number;

  constructor(private childrenitemsService: ChildrenitemsService,
              private  router: Router) {
    this.userParams = this.childrenitemsService.getUserParams();
     }

  ngOnInit(): void {
    this.getChildrenitems();
  }

  getChildrenitems() {
    this.childrenitemsService.setUserParams(this.userParams);
    this.childrenitemsService.getItems(this.userParams)
    .subscribe(response => {
      this.childrenitems = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getChildrenitems();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.childrenitemsService.resetUserParams();
    this.getChildrenitems();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.childrenitemsService.setUserParams(this.userParams);
      this.getChildrenitems();
    }
}

}
