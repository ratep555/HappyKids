import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogsComponent } from './blogs.component';
import { AddBlogComponent } from './add-blog/add-blog.component';
import { EditBlogComponent } from './edit-blog/edit-blog.component';



@NgModule({
  declarations: [
    BlogsComponent,
    AddBlogComponent,
    EditBlogComponent
  ],
  imports: [
    CommonModule
  ]
})
export class BlogsModule { }
