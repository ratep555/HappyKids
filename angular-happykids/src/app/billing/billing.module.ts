import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BillingComponent } from './billing.component';
import { BillingAddressComponent } from './billing-address/billing-address.component';
import { BillingStripeComponent } from './billing-stripe/billing-stripe.component';
import { BillingPaymentoptionComponent } from './billing-paymentoption/billing-paymentoption.component';
import { BillingShippingoptionComponent } from './billing-shippingoption/billing-shippingoption.component';
import { BillingCompletedComponent } from './billing-completed/billing-completed.component';
import { SharedModule } from '../shared/shared.module';
import { BillingRoutingModule } from './billing-routing.module';
import { BillingOrdersummaryComponent } from './billing-ordersummary/billing-ordersummary.component';



@NgModule({
  declarations: [
    BillingComponent,
    BillingAddressComponent,
    BillingStripeComponent,
    BillingPaymentoptionComponent,
    BillingShippingoptionComponent,
    BillingCompletedComponent,
    BillingOrdersummaryComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BillingRoutingModule
  ]
})
export class BillingModule { }
