import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChildrenitemsComponent } from './childrenitems.component';
import { AddChildrenitemComponent } from './add-childrenitem/add-childrenitem.component';
import { EditChildrenitemComponent } from './edit-childrenitem/edit-childrenitem.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { ChildrenitemsRoutingModule } from './childrenitems-routing.module';



@NgModule({
  declarations: [
    ChildrenitemsComponent,
    AddChildrenitemComponent,
    EditChildrenitemComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ChildrenitemsRoutingModule
  ]
})
export class ChildrenitemsModule { }
