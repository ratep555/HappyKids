import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BirthdaysComponent } from './birthdays.component';
import { BirthdaypackageComponent } from './birthdaypackage/birthdaypackage.component';
import { BirthdaypackageDetailComponent } from './birthdaypackage-detail/birthdaypackage-detail.component';
import { SharedModule } from '../shared/shared.module';
import { BirthdaysRoutingModule } from './birthdays-routing.module';
import { AddOrderBirthdaysComponent } from './add-order-birthdays/add-order-birthdays.component';



@NgModule({
  declarations: [
    BirthdaysComponent,
    BirthdaypackageComponent,
    BirthdaypackageDetailComponent,
    AddOrderBirthdaysComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BirthdaysRoutingModule
  ]
})
export class BirthdaysModule { }
