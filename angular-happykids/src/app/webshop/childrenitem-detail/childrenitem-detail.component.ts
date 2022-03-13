import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ChildrenitemsService } from 'src/app/admin/childrenitems/childrenitems.service';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';

@Component({
  selector: 'app-childrenitem-detail',
  templateUrl: './childrenitem-detail.component.html',
  styleUrls: ['./childrenitem-detail.component.scss']
})
export class ChildrenitemDetailComponent implements OnInit {
  childrenItem: ChildrenItem;
  quantity = 1;
  result: number;

  constructor(private childrenitemsService: ChildrenitemsService,
              private activatedRoute: ActivatedRoute,
              private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadChildrenItem();
  }

  loadChildrenItem() {
    this.childrenitemsService.getChildrenItemById(+this.activatedRoute.snapshot.paramMap.get('id'))
    .subscribe(childernItem => {
      this.childrenItem = childernItem;
    }, error => {
      console.log(error);
    });
  }


}







