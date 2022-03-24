import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { KidActivity } from '../shared/models/kidactivity';
import { MyParams } from '../shared/models/myparams';
import { ActivitiesService } from './activities.service';

@Component({
  selector: 'app-activities',
  templateUrl: './activities.component.html',
  styleUrls: ['./activities.component.scss']
})
export class ActivitiesComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  kidActivities: KidActivity[];
  myParams: MyParams;
  totalCount: number;

  constructor(private activitiesService: ActivitiesService) {
    this.myParams = this.activitiesService.getMyParams();
   }

  ngOnInit(): void {
    this.getKidActivities();
  }

  getKidActivities() {
    this.activitiesService.getKidActivities().subscribe(response => {
      this.kidActivities = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  onPageChanged(event: any) {
    const params = this.activitiesService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.activitiesService.setMyParams(params);
      this.getKidActivities();
    }
  }

  onSearch() {
    const params = this.activitiesService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.activitiesService.setMyParams(params);
    this.getKidActivities();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.activitiesService.setMyParams(this.myParams);
    this.getKidActivities();
  }
}

