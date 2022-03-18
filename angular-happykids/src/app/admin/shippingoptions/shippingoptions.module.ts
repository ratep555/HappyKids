import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ShippingoptionsComponent } from './shippingoptions.component';
import { AddShippingoptionComponent } from './add-shippingoption/add-shippingoption.component';
import { EditShippingoptionComponent } from './edit-shippingoption/edit-shippingoption.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ShippingoptionsRoutingModule } from './shippingoptions-routing.module';



@NgModule({
  declarations: [
    ShippingoptionsComponent,
    AddShippingoptionComponent,
    EditShippingoptionComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ShippingoptionsRoutingModule
  ]
})
export class ShippingoptionsModule { }
