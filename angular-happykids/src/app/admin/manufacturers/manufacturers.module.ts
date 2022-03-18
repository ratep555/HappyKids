import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManufacturersComponent } from './manufacturers.component';
import { AddManufacturerComponent } from './add-manufacturer/add-manufacturer.component';
import { EditManufacturerComponent } from './edit-manufacturer/edit-manufacturer.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ManufacturersRoutingModule } from './manufacturers-routing.module';



@NgModule({
  declarations: [
    ManufacturersComponent,
    AddManufacturerComponent,
    EditManufacturerComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ManufacturersRoutingModule
  ]
})
export class ManufacturersModule { }
