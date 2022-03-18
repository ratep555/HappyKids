import { CdkStepper } from '@angular/cdk/stepper';
import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { Basket } from 'src/app/shared/models/basket';
import { Order } from 'src/app/shared/models/order';
import { PaymentOption } from 'src/app/shared/models/paymentOption';
import { BillingService } from '../billing.service';

@Component({
  selector: 'app-billing-ordersummary',
  templateUrl: './billing-ordersummary.component.html',
  styleUrls: ['./billing-ordersummary.component.scss']
})
export class BillingOrdersummaryComponent implements OnInit {
  @Input() billingForm: FormGroup;
  @Input() appStepper: CdkStepper;
  basket$: Observable<Basket>;
  stripe: PaymentOption;

  constructor(private basketService: BasketService,
              private billingService: BillingService,
              private toastr: ToastrService,
              private router: Router) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
    this.billingService.getStripePaymentOption()
    .subscribe(res => this.stripe = res);
  }

   createPaymentIntent() {
      return this.basketService.createPaymentIntent().subscribe((response: any) => {
        this.toastr.success('Payment intent created');
        console.log(this.basketService.gettingValueOfBasket());
        this.appStepper.next();
         }, error => {
          console.log(error);
        });
      }


  createOrder() {
    const basket = this.basketService.gettingValueOfBasket();
    const orderToCreate = this.getOrderToCreate(basket);
    this.billingService.createOrder(orderToCreate).subscribe((order: Order) => {
    this.basketService.deleteClientBasket(basket);
    this.router.navigate(['billing/completed']);
    });
  }

  private getOrderToCreate(basket: Basket) {
    return {
      basketId: basket.id,
      shippingOptionId: +this.billingForm.get('shippingForm').get('shippingOption').value,
      paymentOptionId: +this.billingForm.get('paymentForm').get('paymentOption').value,
      shippingAddress: this.billingForm.get('addressForm').value
    };
  }

}
