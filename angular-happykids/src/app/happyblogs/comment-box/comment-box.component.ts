import { Component, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { BlogCommentClass, BlogCommentClientOnlyClass, BlogCommentCreateEditClass } from 'src/app/shared/models/blogcomment';
import { HappyblogsService } from '../happyblogs.service';

@Component({
  selector: 'app-comment-box',
  templateUrl: './comment-box.component.html',
  styleUrls: ['./comment-box.component.scss']
})
export class CommentBoxComponent implements OnInit {
  @Input() comment: BlogCommentClientOnlyClass;
  @Output() commentSaved = new EventEmitter<BlogCommentClass>();
  @ViewChild('commentForm') commentForm: NgForm;

  constructor(private happyblogsService: HappyblogsService,
              private toastr: ToastrService
  ) { }

  ngOnInit(): void {
  }

  resetComment() {
    this.commentForm.reset();
  }

  onSubmit() {
    const blogCommentCreate: BlogCommentCreateEditClass = {
      id: this.comment.id,
      parentBlogCommentId: this.comment.parentBlogCommentId,
      blogId: this.comment.blogId,
      commentContent: this.comment.commentContent
    };
    this.happyblogsService.upsertBlogComment(blogCommentCreate).subscribe(blogComment => {
      this.toastr.info('Comment saved.');
      this.resetComment();
      this.commentSaved.emit(blogComment);
    });
  }
}
