import { Component, Input, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { AccountService } from 'src/app/account/account.service';
import { BlogCommentClass, BlogCommentClientOnlyClass } from 'src/app/shared/models/blogcomment';
import { User } from 'src/app/shared/models/user';
import { HappyblogsService } from '../happyblogs.service';

@Component({
  selector: 'app-comment-functionality',
  templateUrl: './comment-functionality.component.html',
  styleUrls: ['./comment-functionality.component.scss']
})
export class CommentFunctionalityComponent implements OnInit {
  @Input() blogId: number;
  user: User;
  currentUser$: Observable<User>;
  standAloneComment: BlogCommentClientOnlyClass;
  blogComments: BlogCommentClass[];
  blogCommentsClientOnly: BlogCommentClientOnlyClass[];

  constructor(private happyblogsService: HappyblogsService,
              public accountService: AccountService)
              { this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user); }

  ngOnInit(): void {
    this.currentUser$ = this.accountService.currentUser$;
    this.happyblogsService.getAllBlogComments(this.blogId).subscribe(blogComments => {
            if (this.user) {
              this.initComment(this.user.displayName);
            }
            this.blogComments = blogComments;
            this.blogCommentsClientOnly = [];
            for (let i = 0; i < this.blogComments.length; i++) {
              if (!this.blogComments[i].parentBlogCommentId) {
                this.findCommentReplies(this.blogCommentsClientOnly, i);
              }
            }
          });
  }

  initComment(username: string) {
    this.standAloneComment = {
      parentBlogCommentId: null,
      commentContent: '',
      blogId: this.blogId,
      id: -1,
      username: username,
      publishedOn: null,
      updatedOn: null,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: []
    };
  }

  findCommentReplies(blogCommentsClientOnly: BlogCommentClientOnlyClass[], index: number) {
    const firstElement = this.blogComments[index];
    const newComments: BlogCommentClientOnlyClass[] = [];
    const commentViewModel: BlogCommentClientOnlyClass = {
      parentBlogCommentId: firstElement.parentBlogCommentId || null,
      commentContent: firstElement.commentContent,
      id: firstElement.id,
      blogId: firstElement.blogId,
      username: firstElement.username,
      publishedOn: firstElement.publishedOn,
      updatedOn: firstElement.updatedOn,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: newComments
    };
    blogCommentsClientOnly.push(commentViewModel);
    for (let i = 0; i < this.blogComments.length; i++) {
      if (this.blogComments[i].parentBlogCommentId === firstElement.id) {
      }
    }
  }

  onCommentSaved(blogComment: BlogCommentClass) {
    const commentViewModel: BlogCommentClientOnlyClass = {
      parentBlogCommentId: blogComment.parentBlogCommentId,
      commentContent: blogComment.commentContent,
      blogId: blogComment.blogId,
      id: blogComment.id,
      username: blogComment.username,
      publishedOn: blogComment.publishedOn,
      updatedOn: blogComment.updatedOn,
      isEditable: false,
      deleteConfirm: false,
      isReplying: false,
      comments: []
    };
    this.blogCommentsClientOnly.unshift(commentViewModel);
  }


}



