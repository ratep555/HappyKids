import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChildrenitemWarehousesComponent } from './childrenitem-warehouses.component';
import { AddChildrenitemWarehouseComponent } from './add-childrenitem-warehouse/add-childrenitem-warehouse.component';
import { EditChildrenitemWarehouseComponent } from './edit-childrenitem-warehouse/edit-childrenitem-warehouse.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ChildrenitemWarehousesRoutingModule } from './childrenitem-warehouses-routing.module';



@NgModule({
  declarations: [
    ChildrenitemWarehousesComponent,
    AddChildrenitemWarehouseComponent,
    EditChildrenitemWarehouseComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ChildrenitemWarehousesRoutingModule
  ]
})
export class ChildrenitemWarehousesModule { }
