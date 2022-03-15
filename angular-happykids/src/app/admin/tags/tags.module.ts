import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TagsComponent } from './tags.component';
import { AddTagComponent } from './add-tag/add-tag.component';
import { EditTagComponent } from './edit-tag/edit-tag.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { TagsRoutingModule } from './tags-routing.module';



@NgModule({
  declarations: [
    TagsComponent,
    AddTagComponent,
    EditTagComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    TagsRoutingModule
  ]
})
export class TagsModule { }
