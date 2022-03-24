import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BirthdaypackagesComponent } from './birthdaypackages.component';
import { AddBirthdaypackageComponent } from './add-birthdaypackage/add-birthdaypackage.component';
import { EditBirthdaypackageComponent } from './edit-birthdaypackage/edit-birthdaypackage.component';



const routes: Routes = [
  {path: '', component: BirthdaypackagesComponent},
  {path: 'addbirthdaypackage', component: AddBirthdaypackageComponent},
  {path: 'editbirthdaypackage/:id', component: EditBirthdaypackageComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class BirthdaypackagesRoutingModule { }
