import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UserParams } from 'src/app/shared/models/myparams';
import { Order } from 'src/app/shared/models/order';
import { OrdersChildrenitemsService } from './orders-childrenitems.service';

@Component({
  selector: 'app-orders-childrenitems',
  templateUrl: './orders-childrenitems.component.html',
  styleUrls: ['./orders-childrenitems.component.scss']
})
export class OrdersChildrenitemsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  orders: Order[];
  userParams: UserParams;
  totalCount: number;

  constructor(private ordersChildrenitemsService: OrdersChildrenitemsService,
              private  router: Router) {
              this.userParams = this.ordersChildrenitemsService.getUserParams();
               }

  ngOnInit(): void {
    this.getAllOrdersForClient();
  }

  getAllOrdersForClient() {
    this.ordersChildrenitemsService.setUserParams(this.userParams);
    this.ordersChildrenitemsService.getAllOrdersForChildrenItemsForClient(this.userParams)
    .subscribe(response => {
      this.orders = response.data;
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
    this.getAllOrdersForClient();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.ordersChildrenitemsService.resetUserParams();
    this.getAllOrdersForClient();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.ordersChildrenitemsService.setUserParams(this.userParams);
      this.getAllOrdersForClient();
    }
}

}
