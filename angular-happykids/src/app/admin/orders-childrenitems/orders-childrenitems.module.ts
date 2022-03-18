import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersChildrenitemsComponent } from './orders-childrenitems.component';
import { EditOrderChildrenitemsComponent } from './edit-order-childrenitems/edit-order-childrenitems.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrdersChildrenitemsRoutingModule } from './orders-childrenitems-routing.module';
import { OrderInfoComponent } from './order-info/order-info.component';



@NgModule({
  declarations: [
    OrdersChildrenitemsComponent,
    EditOrderChildrenitemsComponent,
    OrderInfoComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OrdersChildrenitemsRoutingModule
  ]
})
export class OrdersChildrenitemsModule { }
