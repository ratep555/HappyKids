import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Category } from '../shared/models/category';
import { ChildrenItem } from '../shared/models/childrenitem';
import { Manufacturer } from '../shared/models/manufacturer';
import { MyParams } from '../shared/models/myparams';
import { Tag } from '../shared/models/tag';
import { WebshopService } from './webshop.service';

@Component({
  selector: 'app-webshop',
  templateUrl: './webshop.component.html',
  styleUrls: ['./webshop.component.scss']
})
export class WebshopComponent implements OnInit {
  @ViewChild('search', {static: false}) searchTerm: ElementRef;
  @ViewChild('filter', {static: false}) filterTerm: ElementRef;
  @ViewChild('filter1', {static: false}) filterTerm1: ElementRef;
  childrenItems: ChildrenItem[];
  myParams: MyParams;
  totalCount: number;
  manufacturers: Manufacturer[];
  tags: Tag[];
  categories: Category[];

  constructor(private webshopService: WebshopService) {
    this.myParams = this.webshopService.getMyParams();
   }

  ngOnInit(): void {
    this.getChildrenItems();
    this.getManufacturers();
    this.getTags();
    this.getCategories();
  }

  getChildrenItems() {
    this.webshopService.getChildrenItems().subscribe(response => {
      this.childrenItems = response.data;
      this.totalCount = response.count;
    }, error => {
      console.log(error);
    });
  }

  getCategories() {
    this.webshopService.getCategoriesAssociatedWithChildrenItems().subscribe(response => {
    this.categories = response;
    }, error => {
    console.log(error);
    });
    }

  getManufacturers() {
    this.webshopService.getManufacturersAssociatedWithChildrenItems().subscribe(response => {
    this.manufacturers = response;
    }, error => {
    console.log(error);
    });
    }

  getTags() {
    this.webshopService.getTagsAssociatedWithChildrenItems().subscribe(response => {
    this.tags = response;
    }, error => {
    console.log(error);
    });
    }

  onManufacturerSelected(manufacturer1Id: number) {
    const params = this.webshopService.getMyParams();
    params.manufacturerId = manufacturer1Id;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getChildrenItems();
    }

  onCategorySelected(categoryId: number) {
    const params = this.webshopService.getMyParams();
    params.categoryId = categoryId;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getChildrenItems();
    }

  onTagSelected(tagId: number) {
    const params = this.webshopService.getMyParams();
    params.tagId = tagId;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getChildrenItems();
    }

  onPageChanged(event: any) {
    const params = this.webshopService.getMyParams();
    if (params.page !== event) {
      params.page = event;
      this.webshopService.setMyParams(params);
      this.getChildrenItems();
    }
  }

  onSearch() {
    const params = this.webshopService.getMyParams();
    params.query = this.searchTerm.nativeElement.value;
    params.page = 1;
    this.webshopService.setMyParams(params);
    this.getChildrenItems();
  }

  onReset() {
    this.searchTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.webshopService.setMyParams(this.myParams);
    this.getChildrenItems();
  }

  onReset1() {
    this.filterTerm.nativeElement.value = '';
    this.myParams = new MyParams();
    this.webshopService.setMyParams(this.myParams);
    this.getChildrenItems();
  }

  onReset2() {
    this.filterTerm1.nativeElement.value = '';
    this.myParams = new MyParams();
    this.webshopService.setMyParams(this.myParams);
    this.getChildrenItems();
  }

}






