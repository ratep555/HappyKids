import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ClientBirthdayOrder } from 'src/app/shared/models/birthdayorder';
import { UserParams } from 'src/app/shared/models/myparams';
import { OrderStatus } from 'src/app/shared/models/orderStatus';
import { OrdersChildrenitemsService } from '../orders-childrenitems/orders-childrenitems.service';
import { OrdersBirthdaysService } from './orders-birthdays.service';

@Component({
  selector: 'app-orders-birthdays',
  templateUrl: './orders-birthdays.component.html',
  styleUrls: ['./orders-birthdays.component.scss']
})
export class OrdersBirthdaysComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  birthdayOrders: ClientBirthdayOrder[];
  userParams: UserParams;
  totalCount: number;
  orderStatuses: OrderStatus[];

  sortOptions = [
    {name: 'All', value: 'all'},
    {name: 'Pending', value: 'pending'},
    {name: 'Approved', value: 'approved'}
  ];

  constructor(private ordersbirthdaysService: OrdersBirthdaysService,
              private ordersChildrenitemsService: OrdersChildrenitemsService,
              private  router: Router) {
              this.userParams = this.ordersChildrenitemsService.getUserParams();
               }

  ngOnInit(): void {
    this.getAllBirthdayOrders();
    this.getOrderStatuses();
  }

  getAllBirthdayOrders() {
    this.ordersbirthdaysService.setUserParams(this.userParams);
    this.ordersbirthdaysService.getAllBirthdayOrders(this.userParams)
    .subscribe(response => {
      this.birthdayOrders = response.data;
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

  onSortSelected(sort: string) {
      this.userParams.sort = sort;
      this.getAllBirthdayOrders();
    }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getAllBirthdayOrders();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.ordersChildrenitemsService.resetUserParams();
    this.getAllBirthdayOrders();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.ordersChildrenitemsService.setUserParams(this.userParams);
      this.getAllBirthdayOrders();
    }
}

}
