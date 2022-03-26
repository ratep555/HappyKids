import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { MessagesComponent } from './messages.component';
import { EditMessageComponent } from './edit-message/edit-message.component';



const routes: Routes = [
  {path: '', component: MessagesComponent},
  {path: 'editmessage/:id', component: EditMessageComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class MessagesRoutingModule { }
