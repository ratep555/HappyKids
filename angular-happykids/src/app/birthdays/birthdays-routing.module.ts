import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BirthdaysComponent } from './birthdays.component';
import { BirthdaypackageDetailComponent } from './birthdaypackage-detail/birthdaypackage-detail.component';
import { AddOrderBirthdaysComponent } from './add-order-birthdays/add-order-birthdays.component';



const routes: Routes = [
  {path: '', component: BirthdaysComponent},
  {path: 'addbirthday', component: AddOrderBirthdaysComponent},
  {path: ':id', component: BirthdaypackageDetailComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class BirthdaysRoutingModule { }
