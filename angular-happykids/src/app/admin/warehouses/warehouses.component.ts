import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserParams } from 'src/app/shared/models/myparams';
import { Warehouse } from 'src/app/shared/models/warehouse';
import { WarehousesService } from './warehouses.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';

@Component({
  selector: 'app-warehouses',
  templateUrl: './warehouses.component.html',
  styleUrls: ['./warehouses.component.scss']
})
export class WarehousesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  warehouses: Warehouse[];
  userParams: UserParams;
  totalCount: number;

  constructor(private warehousesService: WarehousesService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.warehousesService.getUserParams();
     }

  ngOnInit(): void {
    this.getWarehouses();
  }

  getWarehouses() {
    this.warehousesService.setUserParams(this.userParams);
    this.warehousesService.getWarehouses(this.userParams)
    .subscribe(response => {
      this.warehouses = response.data;
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
    this.getWarehouses();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.warehousesService.resetUserParams();
    this.getWarehouses();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.warehousesService.setUserParams(this.userParams);
      this.getWarehouses();
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
          this.warehousesService.deleteWarehouse(id)
      .subscribe(
        res => {
          this.getWarehouses();
          this.toastr.error('Deleted successfully!');
        }, err => { console.log(err);
         });
    }
  });
  }

}
