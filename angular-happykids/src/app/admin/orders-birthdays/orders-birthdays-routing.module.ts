import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { OrdersBirthdaysComponent } from './orders-birthdays.component';
import { EditOrderBirthdaysComponent } from './edit-order-birthdays/edit-order-birthdays.component';
import { OrderBirthdayInfoComponent } from './order-birthday-info/order-birthday-info.component';



const routes: Routes = [
  {path: '', component: OrdersBirthdaysComponent},
  {path: 'editorderbirthdays/:id', component: EditOrderBirthdaysComponent},
  {path: 'orderbirthdayinfo/:id', component: OrderBirthdayInfoComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrdersBirthdaysRoutingModule { }
