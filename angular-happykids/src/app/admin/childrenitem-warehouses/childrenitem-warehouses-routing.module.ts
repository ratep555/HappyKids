import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ChildrenitemWarehousesComponent } from './childrenitem-warehouses.component';
import { AddChildrenitemWarehouseComponent } from './add-childrenitem-warehouse/add-childrenitem-warehouse.component';
import { EditChildrenitemWarehouseComponent } from './edit-childrenitem-warehouse/edit-childrenitem-warehouse.component';



const routes: Routes = [
  {path: '', component: ChildrenitemWarehousesComponent},
  {path: 'addchildrenitemwarehouse', component: AddChildrenitemWarehouseComponent},
  {path: 'editchildrenitemwarehouse/:id/:warehouseid', component: EditChildrenitemWarehouseComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ChildrenitemWarehousesRoutingModule { }
