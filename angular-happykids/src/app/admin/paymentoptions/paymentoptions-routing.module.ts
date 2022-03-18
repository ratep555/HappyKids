import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { PaymentoptionsComponent } from './paymentoptions.component';
import { AddPaymentoptionComponent } from './add-paymentoption/add-paymentoption.component';
import { EditPaymentoptionComponent } from './edit-paymentoption/edit-paymentoption.component';



const routes: Routes = [
  {path: '', component: PaymentoptionsComponent},
  {path: 'addpaymentoption', component: AddPaymentoptionComponent},
  {path: 'editpaymentoption/:id', component: EditPaymentoptionComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class PaymentoptionsRoutingModule { }
