import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { PaymentOption } from 'src/app/shared/models/paymentOption';
import { BillingService } from '../billing.service';

@Component({
  selector: 'app-billing-paymentoption',
  templateUrl: './billing-paymentoption.component.html',
  styleUrls: ['./billing-paymentoption.component.scss']
})
export class BillingPaymentoptionComponent implements OnInit {
  @Input() billingForm: FormGroup;
  paymentOptions: PaymentOption[];

  constructor(private billingService: BillingService,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.getPaymentOptions();
  }

  getPaymentOptions() {
    this.billingService.getPaymentOptions().subscribe((paymentOptions: PaymentOption[]) => {
      this.paymentOptions = paymentOptions;
    }, error => {
      console.log(error);
    });
  }

  persistingPaymentOption(paymentOptions: PaymentOption) {
    this.basketService.persistingPaymentOption(paymentOptions);
  }

}
