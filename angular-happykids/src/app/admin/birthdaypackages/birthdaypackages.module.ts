import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BirthdaypackagesComponent } from './birthdaypackages.component';
import { AddBirthdaypackageComponent } from './add-birthdaypackage/add-birthdaypackage.component';
import { EditBirthdaypackageComponent } from './edit-birthdaypackage/edit-birthdaypackage.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { BirthdaypackagesRoutingModule } from './birthdaypackages-routing.module';



@NgModule({
  declarations: [
    BirthdaypackagesComponent,
    AddBirthdaypackageComponent,
    EditBirthdaypackageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BirthdaypackagesRoutingModule
  ]
})
export class BirthdaypackagesModule { }
