import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { OrderstatusesComponent } from './orderstatuses.component';
import { AddOrderstatusComponent } from './add-orderstatus/add-orderstatus.component';
import { EditOrderstatusComponent } from './edit-orderstatus/edit-orderstatus.component';



const routes: Routes = [
  {path: '', component: OrderstatusesComponent},
  {path: 'addorderstatus', component: AddOrderstatusComponent},
  {path: 'editorderstatus/:id', component: EditOrderstatusComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class OrderstatusesRoutingModule { }
