import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BirthdaysComponent } from './birthdays.component';
import { BirthdaypackageDetailComponent } from './birthdaypackage-detail/birthdaypackage-detail.component';



const routes: Routes = [
  {path: '', component: BirthdaysComponent},
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
