import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { WarehousesComponent } from './warehouses.component';
import { AddWarehouseComponent } from './add-warehouse/add-warehouse.component';
import { EditWarehouseComponent } from './edit-warehouse/edit-warehouse.component';



const routes: Routes = [
  {path: '', component: WarehousesComponent},
  {path: 'addwarehouse', component: AddWarehouseComponent},
  {path: 'editwarehouse/:id', component: EditWarehouseComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class WarehousesRoutingModule { }
