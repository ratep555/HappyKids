import { Component, OnInit } from '@angular/core';
import { BlogsService } from '../admin/blogs/blogs.service';
import { Blog } from '../shared/models/blog';
import { MyParams } from '../shared/models/myparams';
import { HappyblogsService } from './happyblogs.service';

@Component({
  selector: 'app-happyblogs',
  templateUrl: './happyblogs.component.html',
  styleUrls: ['./happyblogs.component.scss']
})
export class HappyblogsComponent implements OnInit {
  blogs: Blog[];
  myParams: MyParams;
  totalCount: number;

  constructor(private happyblogsService: HappyblogsService) {
    this.myParams = this.happyblogsService.getMyParams();
   }

  ngOnInit(): void {
    this.getBlogs();
  }

  getBlogs() {
    this.happyblogsService.getAllBlogs().subscribe(response => {
      this.blogs = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.happyblogsService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.happyblogsService.setMyParams(params);
      this.getBlogs();
    }
  }

}
