import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UsersListComponent } from './users-list.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { UsersListRoutingModule } from './users-list-routing.module';
import { AddManagerComponent } from './add-manager/add-manager.component';



@NgModule({
  declarations: [
    UsersListComponent,
    AddManagerComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    UsersListRoutingModule
  ]
})
export class UsersListModule { }
