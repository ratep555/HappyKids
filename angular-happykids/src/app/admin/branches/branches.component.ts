import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Branch } from 'src/app/shared/models/branch';
import { UserParams } from 'src/app/shared/models/myparams';
import { BranchesService } from './branches.service';
import Swal from 'sweetalert2/dist/sweetalert2.js';


@Component({
  selector: 'app-branches',
  templateUrl: './branches.component.html',
  styleUrls: ['./branches.component.scss']
})
export class BranchesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  branches: Branch[];
  userParams: UserParams;
  totalCount: number;

  constructor(private branchesService: BranchesService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.branchesService.getUserParams();
     }

  ngOnInit(): void {
    this.getBranches();
  }

  getBranches() {
    this.branchesService.setUserParams(this.userParams);
    this.branchesService.getAllBranches(this.userParams)
    .subscribe(response => {
      this.branches = response.data;
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
    this.getBranches();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.branchesService.resetUserParams();
    this.getBranches();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.branchesService.setUserParams(this.userParams);
      this.getBranches();
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
        this.branchesService.deleteBranch(id)
    .subscribe(
      res => {
        this.getBranches();
        this.toastr.error('Deleted successfully!');
      }, err => { console.log(err);
       });
  }
});
}

}
