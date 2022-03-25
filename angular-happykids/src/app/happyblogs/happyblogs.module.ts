import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HappyblogsComponent } from './happyblogs.component';
import { BlogComponent } from './blog/blog.component';
import { BlogDetailComponent } from './blog-detail/blog-detail.component';
import { SharedModule } from '../shared/shared.module';
import { HappyblogsRoutingModule } from './happyblogs-routing.module';
import { CommentFunctionalityComponent } from './comment-functionality/comment-functionality.component';
import { CommentBoxComponent } from './comment-box/comment-box.component';
import { CommentsComponent } from './comments/comments.component';



@NgModule({
  declarations: [
    HappyblogsComponent,
    BlogComponent,
    BlogDetailComponent,
    CommentFunctionalityComponent,
    CommentBoxComponent,
    CommentsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    HappyblogsRoutingModule
  ]
})
export class HappyblogsModule { }
