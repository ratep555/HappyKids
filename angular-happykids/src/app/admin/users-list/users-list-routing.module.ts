import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersListComponent } from './users-list.component';
import { RouterModule, Routes } from '@angular/router';
import { AddManagerComponent } from './add-manager/add-manager.component';



const routes: Routes = [
  {path: '', component: UsersListComponent},
  {path: 'addmanager', component: AddManagerComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class UsersListRoutingModule { }
