import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { BasketService } from 'src/app/basket/basket.service';
import { ChildrenItem } from 'src/app/shared/models/childrenitem';
import { WebshopService } from '../webshop.service';

@Component({
  selector: 'app-childrenitem',
  templateUrl: './childrenitem.component.html',
  styleUrls: ['./childrenitem.component.scss']
})
export class ChildrenitemComponent implements OnInit {
  @Input() childrenItem: ChildrenItem;

  constructor(private basketService: BasketService,
              private webshopService: WebshopService,
              private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  addingItemToBasket() {
    this.basketService.addingItemToBasket(this.childrenItem);
    this.webshopService.decreaseStockQuantity(this.childrenItem.id, 1).subscribe(() => {
    });
  }


}
