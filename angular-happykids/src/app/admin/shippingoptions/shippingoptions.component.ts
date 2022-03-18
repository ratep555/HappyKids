import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserParams } from 'src/app/shared/models/myparams';
import { ShippingOption } from 'src/app/shared/models/shippingOption';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { ShippingoptionsService } from './shippingoptions.service';


@Component({
  selector: 'app-shippingoptions',
  templateUrl: './shippingoptions.component.html',
  styleUrls: ['./shippingoptions.component.scss']
})
export class ShippingoptionsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  shippingOptions: ShippingOption[];
  userParams: UserParams;
  totalCount: number;

  constructor(private shippingOptionsService: ShippingoptionsService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.shippingOptionsService.getUserParams();
     }

  ngOnInit(): void {
    this.getShippingOptions();
  }

  getShippingOptions() {
    this.shippingOptionsService.setUserParams(this.userParams);
    this.shippingOptionsService.getShippingOptions(this.userParams)
    .subscribe(response => {
      this.shippingOptions = response.data;
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
    this.getShippingOptions();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.shippingOptionsService.resetUserParams();
    this.getShippingOptions();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.shippingOptionsService.setUserParams(this.userParams);
      this.getShippingOptions();
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
          this.shippingOptionsService.deleteShippingOption(id)
      .subscribe(
        res => {
          this.getShippingOptions();
          this.toastr.error('Deleted successfully!');
        }, err => { console.log(err);
         });
    }
  });
  }

}
