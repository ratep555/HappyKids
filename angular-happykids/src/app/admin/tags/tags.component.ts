import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { UserParams } from 'src/app/shared/models/myparams';
import { Tag } from 'src/app/shared/models/tag';
import Swal from 'sweetalert2/dist/sweetalert2.js';
import { TagsService } from './tags.service';

@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.scss']
})
export class TagsComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  tags: Tag[];
  userParams: UserParams;
  totalCount: number;

  constructor(private tagsService: TagsService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.tagsService.getUserParams();
     }

  ngOnInit(): void {
    this.getTags();
  }

  getTags() {
    this.tagsService.setUserParams(this.userParams);
    this.tagsService.getTags(this.userParams)
    .subscribe(response => {
      this.tags = response.data;
      this.userParams.page = response.page;
      this.userParams.pageCount = response.pageCount;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    }
    );
  }

  onSearch() {
    this.userParams.query = this.searchTerm.nativeElement.value;
    this.getTags();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.tagsService.resetUserParams();
    this.getTags();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.tagsService.setUserParams(this.userParams);
      this.getTags();
    }
  }

  onDelete(id: number) {
    Swal.fire({
      title: 'Are you sure want to delete this record?',
      text: 'You will not be able to recover it afterwards!',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Yes, delete it!',
      confirmButtonColor: '#DD6B55',
      cancelButtonText: 'No, keep it'
    }).then((result) => {
      if (result.value) {
          this.tagsService.deleteTag(id)
      .subscribe(
        res => {
          this.getTags();
          this.toastr.error('Deleted successfully!');
        }, err => { console.log(err);
         });
    }
  });
  }

}
