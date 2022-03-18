import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { OrdersChildrenitemsComponent } from './orders-childrenitems.component';
import { OrderInfoComponent } from './order-info/order-info.component';



const routes: Routes = [
  {path: '', component: OrdersChildrenitemsComponent},
  {path: 'orderinfo/:id', component: OrderInfoComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrdersChildrenitemsRoutingModule { }
