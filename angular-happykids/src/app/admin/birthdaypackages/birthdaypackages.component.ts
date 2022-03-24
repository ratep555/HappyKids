import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { BirthdayPackage } from 'src/app/shared/models/birthdaypackage';
import { UserParams } from 'src/app/shared/models/myparams';
import { BirthdaypackagesService } from './birthdaypackages.service';

@Component({
  selector: 'app-birthdaypackages',
  templateUrl: './birthdaypackages.component.html',
  styleUrls: ['./birthdaypackages.component.scss']
})
export class BirthdaypackagesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  birthdaypackages: BirthdayPackage[];
  userParams: UserParams;
  totalCount: number;

  constructor(private birthdaypackagesService: BirthdaypackagesService,
              private  router: Router) {
    this.userParams = this.birthdaypackagesService.getUserParams();
     }

  ngOnInit(): void {
    this.getBirthdayPackages();
  }

  getBirthdayPackages() {
    this.birthdaypackagesService.setUserParams(this.userParams);
    this.birthdaypackagesService.getBirthdayPackages(this.userParams)
    .subscribe(response => {
      this.birthdaypackages = response.data;
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
    this.getBirthdayPackages();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.birthdaypackagesService.resetUserParams();
    this.getBirthdayPackages();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.birthdaypackagesService.setUserParams(this.userParams);
      this.getBirthdayPackages();
    }
}

}
