import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ChildrenitemsService } from 'src/app/admin/childrenitems/childrenitems.service';
import { BasketService } from 'src/app/basket/basket.service';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';
import { WebshopService } from '../webshop.service';
import Swal from 'sweetalert2';


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
              private webshopService: WebshopService,
              private basketService: BasketService,
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

  addingItemToBasket() {
    const result = this.childrenItem.stockQuantity - this.quantity;
    if (result >= 0) {
      this.basketService.addingItemToBasket(this.childrenItem, this.quantity);
    } else {
      this.toastr.error('Insuficient quantity!');
    }
    this.webshopService.decreaseStockQuantity(this.childrenItem.id, this.quantity).subscribe(() => {
      this.loadChildrenItem();
    });
}

increaseQuantity() {
  if (this.childrenItem.stockQuantity > 1 && this.quantity < this.childrenItem.stockQuantity) {
    this.quantity++;
  }
}

decreaseQuantity() {
  if (this.quantity > 1) {
    this.quantity--;
  }
}

onRating(rate: number){
  this.webshopService.ratings(this.childrenItem.id, rate).subscribe(() => {
   Swal.fire('Success', 'Your vote has been received', 'success');
   this.loadChildrenItem();
 });
}

}
