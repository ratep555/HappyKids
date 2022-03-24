import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersBirthdaysComponent } from './orders-birthdays.component';
import { EditOrderBirthdaysComponent } from './edit-order-birthdays/edit-order-birthdays.component';
import { OrderBirthdayInfoComponent } from './order-birthday-info/order-birthday-info.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrdersBirthdaysRoutingModule } from './orders-birthdays-routing.module';



@NgModule({
  declarations: [
    OrdersBirthdaysComponent,
    EditOrderBirthdaysComponent,
    OrderBirthdayInfoComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OrdersBirthdaysRoutingModule
  ]
})
export class OrdersBirthdaysModule { }
