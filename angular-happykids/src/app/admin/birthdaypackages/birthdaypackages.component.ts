import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { BirthdayPackage } from 'src/app/shared/models/birthdaypackage';
import { UserParams } from 'src/app/shared/models/myparams';
import { BirthdaypackagesService } from './birthdaypackages.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ToastrService } from 'ngx-toastr';


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
              private toastr: ToastrService,
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

onDelete(id: number) {
  Swal.fire({
    title: 'Are you sure want to delete this record?',
    text: 'You will not be able to recover it afterwards!',
    icon: 'warning',
    showCancelButton: true,
    confirmButtonText: 'Yes, delete it!',
    confirmButtonColor: '#DD6B55',
    cancelButtonText: 'No, keep it'
  }).then((result) => {
    if (result.value) {
        this.birthdaypackagesService.deleteBirthdayPackage(id)
    .subscribe(
      res => {
        this.getBirthdayPackages();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}


}
