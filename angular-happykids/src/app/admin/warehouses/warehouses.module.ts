import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AddWarehouseComponent } from './add-warehouse/add-warehouse.component';
import { EditWarehouseComponent } from './edit-warehouse/edit-warehouse.component';
import { WarehousesComponent } from './warehouses.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { WarehousesRoutingModule } from './warehouses-routing.module';



@NgModule({
  declarations: [
    AddWarehouseComponent,
    EditWarehouseComponent,
    WarehousesComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WarehousesRoutingModule
  ]
})
export class WarehousesModule { }
