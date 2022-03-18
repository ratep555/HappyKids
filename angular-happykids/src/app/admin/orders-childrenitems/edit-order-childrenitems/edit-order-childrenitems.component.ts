import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { first } from 'rxjs/operators';
import { Order } from 'src/app/shared/models/order';
import { OrdersChildrenitemsService } from '../orders-childrenitems.service';

@Component({
  selector: 'app-edit-order-childrenitems',
  templateUrl: './edit-order-childrenitems.component.html',
  styleUrls: ['./edit-order-childrenitems.component.scss']
})
export class EditOrderChildrenitemsComponent implements OnInit {
  orderForm: FormGroup;
  model: Order;
  orderstatusesList = [];
  id: number;

  constructor(public ordersChildrenitemsService: OrdersChildrenitemsService,
              private router: Router,
              private fb: FormBuilder,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.id = this.activatedRoute.snapshot.params['id'];

    this.ordersChildrenitemsService.getOrderById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe((order: Order) => {
    this.model = order;
    }, error => {
    console.log(error);
    });

    this.ordersChildrenitemsService.getOrderStatuses()
    .subscribe(res => this.orderstatusesList = res as []);

    this.orderForm = this.fb.group({
      id: [this.id],
      orderStatusId: [null],
     });

    this.ordersChildrenitemsService.getOrderById(this.id)
    .pipe(first())
    .subscribe(x => this.orderForm.patchValue(x));
  }

  onSubmit() {
    if (this.orderForm.invalid) {
        return;
    }
    this.updateOrder();
  }

  private updateOrder() {
    this.ordersChildrenitemsService.updateOrder(this.id, this.orderForm.value)
        .pipe(first())
        .subscribe(() => {
          this.router.navigateByUrl('orderschildrenitems');
          }, error => {
            console.log(error);
          });
        }

}
