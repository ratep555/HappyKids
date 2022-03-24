import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivitiesComponent } from './activities.component';
import { KidactivityComponent } from './kidactivity/kidactivity.component';
import { KidactivityDetailComponent } from './kidactivity-detail/kidactivity-detail.component';
import { SharedModule } from '../shared/shared.module';
import { ActivitiesRoutingModule } from './activities-routing.module';



@NgModule({
  declarations: [
    ActivitiesComponent,
    KidactivityComponent,
    KidactivityDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ActivitiesRoutingModule
  ]
})
export class ActivitiesModule { }
