import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BlogsService } from 'src/app/admin/blogs/blogs.service';
import { Blog } from 'src/app/shared/models/blog';

@Component({
  selector: 'app-blog-detail',
  templateUrl: './blog-detail.component.html',
  styleUrls: ['./blog-detail.component.scss']
})
export class BlogDetailComponent implements OnInit {
  blog: Blog;

  constructor(private blogsService: BlogsService,
              private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.loadBlogs();
  }

  loadBlogs() {
    return this.blogsService.getBlogById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(response => {
    this.blog = response;
    }, error => {
    console.log(error);
    });
    }

}
