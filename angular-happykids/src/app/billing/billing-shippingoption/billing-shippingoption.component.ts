import { Component, Input, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { BasketService } from 'src/app/basket/basket.service';
import { ShippingOption } from 'src/app/shared/models/shippingOption';
import { BillingService } from '../billing.service';

@Component({
  selector: 'app-billing-shippingoption',
  templateUrl: './billing-shippingoption.component.html',
  styleUrls: ['./billing-shippingoption.component.scss']
})
export class BillingShippingoptionComponent implements OnInit {
  @Input() billingForm: FormGroup;
  shippingOptions: ShippingOption[];

  constructor(private billingService: BillingService,
              private basketService: BasketService) { }

  ngOnInit(): void {
    this.getShippingOptions();
  }

  getShippingOptions() {
    this.billingService.getShippingOptions().subscribe((shippingOptions: ShippingOption[]) => {
      this.shippingOptions = shippingOptions;
    }, error => {
      console.log(error);
    });
  }

  calculateShippingPrice(shippingOption: ShippingOption) {
    this.basketService.calculateShippingPrice(shippingOption);
  }

}
