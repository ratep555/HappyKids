import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersChildrenitemsComponent } from './orders-childrenitems.component';
import { OrderInfoComponent } from './order-info/order-info.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrdersChildrenitemsRoutingModule } from './orders-childrenitems-routing.module';



@NgModule({
  declarations: [
    OrdersChildrenitemsComponent,
    OrderInfoComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OrdersChildrenitemsRoutingModule
  ]
})
export class OrdersChildrenitemsModule { }
