import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BranchesComponent } from './branches.component';
import { AddBranchComponent } from './add-branch/add-branch.component';
import { EditBranchComponent } from './edit-branch/edit-branch.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { BranchesRoutingModule } from './branches-routing.module';



@NgModule({
  declarations: [
    BranchesComponent,
    AddBranchComponent,
    EditBranchComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BranchesRoutingModule
  ]
})
export class BranchesModule { }
