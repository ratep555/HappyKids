import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { KidactivitiesComponent } from './kidactivities.component';
import { AddKidactivityComponent } from './add-kidactivity/add-kidactivity.component';
import { EditKidactivityComponent } from './edit-kidactivity/edit-kidactivity.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { KidactivitiesRoutingModule } from './kidactivities-routing.module';



@NgModule({
  declarations: [
    KidactivitiesComponent,
    AddKidactivityComponent,
    EditKidactivityComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    KidactivitiesRoutingModule
  ]
})
export class KidactivitiesModule { }
