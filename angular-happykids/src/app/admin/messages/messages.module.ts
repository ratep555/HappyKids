import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MessagesComponent } from './messages.component';
import { EditMessageComponent } from './edit-message/edit-message.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { MessagesRoutingModule } from './messages-routing.module';



@NgModule({
  declarations: [
    MessagesComponent,
    EditMessageComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    MessagesRoutingModule
  ]
})
export class MessagesModule { }
