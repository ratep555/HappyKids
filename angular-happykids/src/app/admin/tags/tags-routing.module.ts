import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { TagsComponent } from './tags.component';
import { AddTagComponent } from './add-tag/add-tag.component';
import { EditTagComponent } from './edit-tag/edit-tag.component';



const routes: Routes = [
  {path: '', component: TagsComponent},
  {path: 'addtag', component: AddTagComponent},
  {path: 'edittag/:id', component: EditTagComponent}
];

@NgModule({
  declarations: [],
  imports: [
    RouterModule.forChild(routes)
  ],
  exports: [RouterModule]
})
export class TagsRoutingModule { }
