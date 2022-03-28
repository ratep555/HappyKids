import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserParams } from 'src/app/shared/models/myparams';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { OrderstatusesService } from './orderstatuses.service';


@Component({
  selector: 'app-orderstatuses',
  templateUrl: './orderstatuses.component.html',
  styleUrls: ['./orderstatuses.component.scss']
})
export class OrderstatusesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  orderstatuses: OrderStatus[];
  userParams: UserParams;
  totalCount: number;

  constructor(private orderstatusesService: OrderstatusesService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.orderstatusesService.getUserParams();
     }

  ngOnInit(): void {
    this.getOrderStatuses();
  }

  getOrderStatuses() {
    this.orderstatusesService.setUserParams(this.userParams);
    this.orderstatusesService.getOrderStatuses(this.userParams)
    .subscribe(response => {
      this.orderstatuses = response.data;
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
    this.getOrderStatuses();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.orderstatusesService.resetUserParams();
    this.getOrderStatuses();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.orderstatusesService.setUserParams(this.userParams);
      this.getOrderStatuses();
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
          this.orderstatusesService.deleteOrderStatus(id)
      .subscribe(
        res => {
          this.getOrderStatuses();
          this.toastr.error('Deleted successfully!');
        }, err => { console.log(err);
         });
    }
  });
  }

}
