import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaymentoptionsComponent } from './paymentoptions.component';
import { AddPaymentoptionComponent } from './add-paymentoption/add-paymentoption.component';
import { EditPaymentoptionComponent } from './edit-paymentoption/edit-paymentoption.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { PaymentoptionsRoutingModule } from './paymentoptions-routing.module';



@NgModule({
  declarations: [
    PaymentoptionsComponent,
    AddPaymentoptionComponent,
    EditPaymentoptionComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    PaymentoptionsRoutingModule
  ]
})
export class PaymentoptionsModule { }
