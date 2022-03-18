import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ManufacturersComponent } from './manufacturers.component';
import { AddManufacturerComponent } from './add-manufacturer/add-manufacturer.component';
import { EditManufacturerComponent } from './edit-manufacturer/edit-manufacturer.component';



const routes: Routes = [
  {path: '', component: ManufacturersComponent},
  {path: 'addmanufacturer', component: AddManufacturerComponent},
  {path: 'editmanufacturer/:id', component: EditManufacturerComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ManufacturersRoutingModule { }
