import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { WebshopComponent } from './webshop.component';
import { ChildrenitemComponent } from './childrenitem/childrenitem.component';
import { ChildrenitemDetailComponent } from './childrenitem-detail/childrenitem-detail.component';
import { SharedModule } from '../shared/shared.module';
import { WebshopRoutingModule } from './webshop-routing.module';



@NgModule({
  declarations: [
    WebshopComponent,
    ChildrenitemComponent,
    ChildrenitemDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    WebshopRoutingModule
  ]
})
export class WebshopModule { }
