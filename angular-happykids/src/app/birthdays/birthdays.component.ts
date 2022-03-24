import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { BirthdayPackage } from '../shared/models/birthdaypackage';
import { MyParams } from '../shared/models/myparams';
import { BirthdaysService } from './birthdays.service';

@Component({
  selector: 'app-birthdays',
  templateUrl: './birthdays.component.html',
  styleUrls: ['./birthdays.component.scss']
})
export class BirthdaysComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  birthdayPackages: BirthdayPackage[];
  myParams: MyParams;
  totalCount: number;

  constructor(private birthdaysService: BirthdaysService) {
    this.myParams = this.birthdaysService.getMyParams();
   }

  ngOnInit(): void {
    this.getBirthdayPackages();
  }

  getBirthdayPackages() {
    this.birthdaysService.getBirthdayPackages().subscribe(response => {
      this.birthdayPackages = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.birthdaysService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.birthdaysService.setMyParams(params);
      this.getBirthdayPackages();
    }
  }

  onSearch() {
    const params = this.birthdaysService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.birthdaysService.setMyParams(params);
    this.getBirthdayPackages();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.birthdaysService.setMyParams(this.myParams);
    this.getBirthdayPackages();
  }
}


