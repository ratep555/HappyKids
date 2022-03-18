import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Category } from 'src/app/shared/models/category';
import { UserParams } from 'src/app/shared/models/myparams';
import { CategoriesService } from './categories.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';


@Component({
  selector: 'app-categories',
  templateUrl: './categories.component.html',
  styleUrls: ['./categories.component.scss']
})
export class CategoriesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  categories: Category[];
  userParams: UserParams;
  totalCount: number;

  constructor(private categoriesService: CategoriesService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.categoriesService.getUserParams();
     }

  ngOnInit(): void {
    this.getCategories();
  }

  getCategories() {
    this.categoriesService.setUserParams(this.userParams);
    this.categoriesService.getCategories(this.userParams)
    .subscribe(response => {
      this.categories = response.data;
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
    this.getCategories();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.categoriesService.resetUserParams();
    this.getCategories();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.categoriesService.setUserParams(this.userParams);
      this.getCategories();
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
          this.categoriesService.deleteCategory(id)
      .subscribe(
        res => {
          this.getCategories();
          this.toastr.error('Deleted successfully!');
        }, err => { console.log(err);
         });
    }
  });
  }

}
