import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { KidActivity } from 'src/app/shared/models/kidactivity';
import { UserParams } from 'src/app/shared/models/myparams';
import { KidactivitiesService } from './kidactivities.service';

@Component({
  selector: 'app-kidactivities',
  templateUrl: './kidactivities.component.html',
  styleUrls: ['./kidactivities.component.scss']
})
export class KidactivitiesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  kidActivities: KidActivity[];
  userParams: UserParams;
  totalCount: number;

  constructor(private kidactivitiesService: KidactivitiesService,
              private  router: Router,
              private toastr: ToastrService) {
    this.userParams = this.kidactivitiesService.getUserParams();
     }

  ngOnInit(): void {
    this.getAllKidActivities();
  }

  getAllKidActivities() {
    this.kidactivitiesService.setUserParams(this.userParams);
    this.kidactivitiesService.getAllKidActivities(this.userParams)
    .subscribe(response => {
      this.kidActivities = response.data;
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
    this.getAllKidActivities();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.userParams = this.kidactivitiesService.resetUserParams();
    this.getAllKidActivities();
  }

  onPageChanged(event: any) {
    if (this.userParams.page !== event) {
      this.userParams.page = event;
      this.kidactivitiesService.setUserParams(this.userParams);
      this.getAllKidActivities();
    }
}

}
