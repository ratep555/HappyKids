import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';
import { WebshopService } from 'src/app/webshop/webshop.service';
import { Basket, BasketChildrenItem } from '../../models/basket';

@Component({
  selector: 'app-basket-review',
  templateUrl: './basket-review.component.html',
  styleUrls: ['./basket-review.component.scss']
})
export class BasketReviewComponent implements OnInit {
  basket$: Observable<Basket>;
  @Output() decrease: EventEmitter<BasketChildrenItem> = new EventEmitter<BasketChildrenItem>();
  @Output() increase: EventEmitter<BasketChildrenItem> = new EventEmitter<BasketChildrenItem>();
  @Output() remove: EventEmitter<BasketChildrenItem> = new EventEmitter<BasketChildrenItem>();
  @Input() isBasket = true;
  @Input() isOrder = false;

  constructor(private basketService: BasketService,
              private webshopservice: WebshopService) { }

  ngOnInit() {
    this.basket$ = this.basketService.basket$;
  }

  decreaseBasketItemQuantity(item: BasketChildrenItem) {
    this.webshopservice.increaseStockQuantity(item.id, 1).subscribe(() => {
    });
    this.decrease.emit(item);
  }

  increaseBasketItemQuantity(item: BasketChildrenItem) {
    if (item.stockQuantity > item.quantity) {
      this.webshopservice.decreaseStockQuantity(item.id, 1).subscribe(() => {
      });
      this.increase.emit(item);
    }
  }

  removingItemFromBasket(item: BasketChildrenItem) {
    this.webshopservice.increaseStockQuantity(item.id, item.quantity).subscribe(() => {
    });
    this.remove.emit(item);
  }

}
