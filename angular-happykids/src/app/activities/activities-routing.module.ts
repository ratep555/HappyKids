import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { ActivitiesComponent } from './activities.component';
import { KidactivityDetailComponent } from './kidactivity-detail/kidactivity-detail.component';



const routes: Routes = [
  {path: '', component: ActivitiesComponent},
  {path: ':id', component: KidactivityDetailComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class ActivitiesRoutingModule { }
