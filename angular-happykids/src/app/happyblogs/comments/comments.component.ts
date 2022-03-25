import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BlogCommentClass, BlogCommentClientOnlyClass } from 'src/app/shared/models/blogcomment';
import { User } from 'src/app/shared/models/user';
import { HappyblogsService } from '../happyblogs.service';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  @Input() comments: BlogCommentClientOnlyClass[];
  currentUser$: Observable<User>;
  user: User;

  constructor(public accountService: AccountService,
              private toastr: ToastrService,
              private happyblogsService: HappyblogsService)
              { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
  }

  editComment(comment: BlogCommentClientOnlyClass) {
    comment.isEditable = true;
  }

  showDeleteConfirm(comment: BlogCommentClientOnlyClass) {
    comment.deleteConfirm = true;
  }

  cancelDeleteConfirm(comment: BlogCommentClientOnlyClass) {
    comment.deleteConfirm = false;
  }

  deleteConfirm(comment: BlogCommentClientOnlyClass, comments: BlogCommentClientOnlyClass[]) {
    this.happyblogsService.deleteBlogComment(comment.id).subscribe(() => {
      let index = 0;
      for (let i = 0; i < comments.length; i++) {
        if (comments[i].id === comment.id) {
          index = i;
        }
      }
      if (index > -1) {
        comments.splice(index, 1);
      }
      this.toastr.info('Blog comment deleted.');
    });
  }

  replyComment(comment: BlogCommentClientOnlyClass) {
    const replyComment: BlogCommentClientOnlyClass = {
      parentBlogCommentId: comment.id,
      commentContent: '',
      blogId: comment.blogId,
      id: -1,
      username: this.user.displayName,
      publishedOn: new Date(),
      updatedOn: new Date(),
      isEditable: false,
      deleteConfirm: false,
      isReplying: true,
      comments: []
    };
    comment.comments.push(replyComment);
  }

  onCommentSaved(blogComment: BlogCommentClass, comment: BlogCommentClientOnlyClass) {
    comment.id = blogComment.id;
    comment.parentBlogCommentId = blogComment.parentBlogCommentId;
    comment.blogId = blogComment.blogId;
    comment.commentContent = blogComment.commentContent;
    comment.publishedOn = blogComment.publishedOn;
    comment.updatedOn = blogComment.updatedOn;
    comment.username = blogComment.username;
    comment.isEditable = false;
    comment.isReplying = false;
  }
}
