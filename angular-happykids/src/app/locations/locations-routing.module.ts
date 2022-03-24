import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { LocationsComponent } from './locations.component';
import { LocationDetailComponent } from './location-detail/location-detail.component';
import { MessageComponent } from './message/message.component';



const routes: Routes = [
  {path: '', component: LocationsComponent},
  {path: 'message', component: MessageComponent},
  {path: ':id', component: LocationDetailComponent},
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class LocationsRoutingModule { }
