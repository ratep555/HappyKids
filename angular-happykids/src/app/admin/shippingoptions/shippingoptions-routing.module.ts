import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ShippingoptionsComponent } from './shippingoptions.component';
import { AddShippingoptionComponent } from './add-shippingoption/add-shippingoption.component';
import { EditShippingoptionComponent } from './edit-shippingoption/edit-shippingoption.component';



const routes: Routes = [
  {path: '', component: ShippingoptionsComponent},
  {path: 'addshippingoption', component: AddShippingoptionComponent},
  {path: 'editshippingoption/:id', component: EditShippingoptionComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ShippingoptionsRoutingModule { }
