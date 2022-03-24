import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocationsComponent } from './locations.component';
import { MessageComponent } from './message/message.component';
import { LocationDetailComponent } from './location-detail/location-detail.component';
import { SharedModule } from '../shared/shared.module';
import { LocationsRoutingModule } from './locations-routing.module';



@NgModule({
  declarations: [
    LocationsComponent,
    MessageComponent,
    LocationDetailComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    LocationsRoutingModule
  ]
})
export class LocationsModule { }
