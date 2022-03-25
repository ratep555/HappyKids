import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BlogsComponent } from './blogs.component';
import { AddBlogComponent } from './add-blog/add-blog.component';
import { EditBlogComponent } from './edit-blog/edit-blog.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { BlogsRoutingModule } from './blogs-routing.module';



@NgModule({
  declarations: [
    BlogsComponent,
    AddBlogComponent,
    EditBlogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    BlogsRoutingModule
  ]
})
export class BlogsModule { }
