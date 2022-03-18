import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserParams } from 'src/app/shared/models/myparams';
import { PaymentOption } from 'src/app/shared/models/paymentOption';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { PaymentoptionsService } from './paymentoptions.service';


@Component({
  selector: 'app-paymentoptions',
  templateUrl: './paymentoptions.component.html',
  styleUrls: ['./paymentoptions.component.scss']
})
export class PaymentoptionsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  paymentOptions: PaymentOption[];
  userParams: UserParams;
  totalCount: number;

  constructor(private paymentOptionsService: PaymentoptionsService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.paymentOptionsService.getUserParams();
     }

  ngOnInit(): void {
    this.getPaymentOptions();
  }

  getPaymentOptions() {
    this.paymentOptionsService.setUserParams(this.userParams);
    this.paymentOptionsService.getPaymentOptions(this.userParams)
    .subscribe(response => {
      this.paymentOptions = response.data;
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
    this.getPaymentOptions();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.paymentOptionsService.resetUserParams();
    this.getPaymentOptions();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.paymentOptionsService.setUserParams(this.userParams);
      this.getPaymentOptions();
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
          this.paymentOptionsService.deletePaymentOption(id)
      .subscribe(
        res => {
          this.getPaymentOptions();
          this.toastr.error('Deleted successfully!');
        }, err => { console.log(err);
         });
    }
  });
  }

}

