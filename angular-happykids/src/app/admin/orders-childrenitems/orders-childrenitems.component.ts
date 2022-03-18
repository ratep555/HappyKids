import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { UserParams } from 'src/app/shared/models/myparams';
import { Order } from 'src/app/shared/models/order';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { OrdersChildrenitemsService } from './orders-childrenitems.service';

@Component({
  selector: 'app-orders-childrenitems',
  templateUrl: './orders-childrenitems.component.html',
  styleUrls: ['./orders-childrenitems.component.scss']
})
export class OrdersChildrenitemsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  orders: Order[];
  userParams: UserParams;
  totalCount: number;
  orderStatuses: OrderStatus[];

  constructor(private ordersChildrenitemsService: OrdersChildrenitemsService,
              private  router: Router) {
              this.userParams = this.ordersChildrenitemsService.getUserParams();
               }

  ngOnInit(): void {
    this.getAllOrders();
    this.getOrderStatuses();
  }

  getAllOrders() {
    this.ordersChildrenitemsService.setUserParams(this.userParams);
    this.ordersChildrenitemsService.getAllOrdersForChildrenItems(this.userParams)
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

  getOrderStatuses() {
    this.ordersChildrenitemsService.getOrderStatusesAssociatedWithOrdersForChildrenItems()
    .subscribe(response => {
    this.orderStatuses = response;
    }, error => {
    console.log(error);
    });
    }

  onOrderStatusSelected(orderStatusId: number) {
    this.userParams.orderStatusId = orderStatusId;
    this.getAllOrders();
    }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getAllOrders();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.ordersChildrenitemsService.resetUserParams();
    this.getAllOrders();
  }

  onReset1() {
    this.filterTerm.nativeElement.value = '';
    this.userParams = this.ordersChildrenitemsService.resetUserParams();
    this.getAllOrders();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.ordersChildrenitemsService.setUserParams(this.userParams);
      this.getAllOrders();
    }
}

}
