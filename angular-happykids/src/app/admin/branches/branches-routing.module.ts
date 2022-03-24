import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { BranchesComponent } from './branches.component';
import { AddBranchComponent } from './add-branch/add-branch.component';
import { EditBranchComponent } from './edit-branch/edit-branch.component';



const routes: Routes = [
  {path: '', component: BranchesComponent},
  {path: 'addbranch', component: AddBranchComponent},
  {path: 'editbranch/:id', component: EditBranchComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class BranchesRoutingModule { }
