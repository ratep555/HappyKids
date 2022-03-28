import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderstatusesComponent } from './orderstatuses.component';
import { AddOrderstatusComponent } from './add-orderstatus/add-orderstatus.component';
import { EditOrderstatusComponent } from './edit-orderstatus/edit-orderstatus.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { OrderstatusesRoutingModule } from './orderstatuses-routing.module';



@NgModule({
  declarations: [
    OrderstatusesComponent,
    AddOrderstatusComponent,
    EditOrderstatusComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    OrderstatusesRoutingModule
  ]
})
export class OrderstatusesModule { }
