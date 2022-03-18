import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Order } from 'src/app/shared/models/order';
import { OrdersChildrenitemsService } from '../orders-childrenitems.service';

@Component({
  selector: 'app-order-info',
  templateUrl: './order-info.component.html',
  styleUrls: ['./order-info.component.scss']
})
export class OrderInfoComponent implements OnInit {
  order: Order;

  constructor(private ordersChildrenitemsService: OrdersChildrenitemsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.getOrderInfo();
  }

  getOrderInfo() {
    this.ordersChildrenitemsService.getOrderById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe((order: Order) => {
      this.order = order;
    }, error => {
      console.log(error);
    });
  }

}
