import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ChildrenItemWarehouse } from 'src/app/shared/models/childrenItemWarehouse';
import { UserParams } from 'src/app/shared/models/myparams';
import { ChildrenitemWarehousesService } from './childrenitem-warehouses.service';

@Component({
  selector: 'app-childrenitem-warehouses',
  templateUrl: './childrenitem-warehouses.component.html',
  styleUrls: ['./childrenitem-warehouses.component.scss']
})
export class ChildrenitemWarehousesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  childrenItemWarehouses: ChildrenItemWarehouse[];
  userParams: UserParams;
  totalCount: number;

  constructor(private childrenItemWarehousesService: ChildrenitemWarehousesService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.childrenItemWarehousesService.getUserParams();
     }

  ngOnInit(): void {
    this.getChildrenItemWarehouses();
  }

  getChildrenItemWarehouses() {
    this.childrenItemWarehousesService.setUserParams(this.userParams);
    this.childrenItemWarehousesService.getChildrenItemWarehouses(this.userParams)
    .subscribe(response => {
      this.childrenItemWarehouses = response.data;
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
    this.getChildrenItemWarehouses();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.childrenItemWarehousesService.resetUserParams();
    this.getChildrenItemWarehouses();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.childrenItemWarehousesService.setUserParams(this.userParams);
      this.getChildrenItemWarehouses();
    }
}

}
