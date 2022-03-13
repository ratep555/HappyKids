import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ChildrenitemsComponent } from './childrenitems.component';
import { AddChildrenitemComponent } from './add-childrenitem/add-childrenitem.component';
import { EditChildrenitemComponent } from './edit-childrenitem/edit-childrenitem.component';



const routes: Routes = [
  {path: '', component: ChildrenitemsComponent},
  {path: 'addchildrenitem', component: AddChildrenitemComponent},
  {path: 'editchildrenitem/:id', component: EditChildrenitemComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ChildrenitemsRoutingModule { }
