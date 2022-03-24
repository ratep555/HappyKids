import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { KidactivitiesComponent } from './kidactivities.component';
import { AddKidactivityComponent } from './add-kidactivity/add-kidactivity.component';
import { EditKidactivityComponent } from './edit-kidactivity/edit-kidactivity.component';



const routes: Routes = [
  {path: '', component: KidactivitiesComponent},
  {path: 'addkidactivity', component: AddKidactivityComponent},
  {path: 'editkidactivity/:id', component: EditKidactivityComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class KidactivitiesRoutingModule { }
