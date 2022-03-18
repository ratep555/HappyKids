import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Manufacturer } from 'src/app/shared/models/manufacturer';
import { UserParams } from 'src/app/shared/models/myparams';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ManufacturersService } from './manufacturers.service';

@Component({
  selector: 'app-manufacturers',
  templateUrl: './manufacturers.component.html',
  styleUrls: ['./manufacturers.component.scss']
})
export class ManufacturersComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  manufacturers: Manufacturer[];
  userParams: UserParams;
  totalCount: number;

  constructor(private manufacturersService: ManufacturersService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.manufacturersService.getUserParams();
     }

  ngOnInit(): void {
    this.getManufacturers();
  }

  getManufacturers() {
    this.manufacturersService.setUserParams(this.userParams);
    this.manufacturersService.getManufacturers(this.userParams)
    .subscribe(response => {
      this.manufacturers = response.data;
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
    this.getManufacturers();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.manufacturersService.resetUserParams();
    this.getManufacturers();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.manufacturersService.setUserParams(this.userParams);
      this.getManufacturers();
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
          this.manufacturersService.deleteManufacturer(id)
      .subscribe(
        res => {
          this.getManufacturers();
          this.toastr.error('Deleted successfully!');
        }, err => { console.log(err);
         });
    }
  });
  }

}
