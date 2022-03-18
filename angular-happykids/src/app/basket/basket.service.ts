import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Basket, BasketChildrenItem, BasketClass, BasketSum } from '../shared/models/basket';
import { ChildrenItem } from '../shared/models/childrenitem';
import { PaymentOption } from '../shared/models/paymentOption';
import { ShippingOption } from '../shared/models/shippingOption';
import { WebshopService } from '../webshop/webshop.service';

@Injectable({
  providedIn: 'root'
})
export class BasketService {
  baseUrl = environment.apiUrl;
  private basketBSubject = new BehaviorSubject<Basket>(null);
  basket$ = this.basketBSubject.asObservable();
  private basketSumBSubject = new BehaviorSubject<BasketSum>(null);
  basketSum$ = this.basketSumBSubject.asObservable();
  shipping = 0;

  constructor(private http: HttpClient, private webshopService: WebshopService) { }

  getClientBasket(id: string) {
    return this.http.get(this.baseUrl + 'baskets?id=' + id)
      .pipe(
        map((basket: Basket) => {
          this.basketBSubject.next(basket);
          this.shipping = basket.shippingPrice;
          this.calculateBasketSum();
          console.log(basket);
        })
      );
  }

  updateClientBasket(basket: Basket) {
    return this.http.post(this.baseUrl + 'baskets', basket).subscribe((response: Basket) => {
      this.basketBSubject.next(response);
      this.calculateBasketSum();
    }, error => {
      console.log(error);
    });
  }

  gettingValueOfBasket() {
    return this.basketBSubject.value;
  }

  addingItemToBasket(childrenItem: ChildrenItem, quantity = 1) {
    const addedItem: BasketChildrenItem = this.mapItemToBasketItem(childrenItem, quantity);
    const basket = this.gettingValueOfBasket() ?? this.creatingBasket();
    basket.basketChildrenItems = this.addingOrUpdatingBasketItem(basket.basketChildrenItems, addedItem, quantity);
    this.updateClientBasket(basket);
  }

  increaseBasketItemQuantity(basketChildrenItem: BasketChildrenItem) {
    const basket = this.gettingValueOfBasket();
    const itemIndex = basket.basketChildrenItems.findIndex(x => x.id === basketChildrenItem.id);
    basket.basketChildrenItems[itemIndex].quantity++;
    this.updateClientBasket(basket);
  }

  decreaseBasketItemQuantity(basketChildrenItem: BasketChildrenItem) {
    const basket = this.gettingValueOfBasket();
    const itemIndex = basket.basketChildrenItems.findIndex(x => x.id === basketChildrenItem.id);
    if (basket.basketChildrenItems[itemIndex].quantity > 1) {
      basket.basketChildrenItems[itemIndex].quantity--;
      this.updateClientBasket(basket);
    } else {
      this.removingItemFromBasket(basketChildrenItem);
    }
  }

  removingItemFromBasket(basketChildrenitem: BasketChildrenItem) {
    const basket = this.gettingValueOfBasket();
    if (basket.basketChildrenItems.some(x => x.id === basketChildrenitem.id)) {
      basket.basketChildrenItems = basket.basketChildrenItems.filter(i => i.id !== basketChildrenitem.id);
      if (basket.basketChildrenItems.length > 0) {
        this.updateClientBasket(basket);
      } else {
        this.deleteClientBasket(basket);
      }
    }
  }

  deleteClientBasket(basket: Basket) {
    return this.http.delete(this.baseUrl + 'baskets?id=' + basket.id).subscribe(() => {
      this.basketBSubject.next(null);
      this.basketSumBSubject.next(null);
      localStorage.removeItem('basket_id');
    }, error => {
      console.log(error);
    });
  }

  deleteClientBasketLocally(id: string) {
    this.basketBSubject.next(null);
    this.basketSumBSubject.next(null);
    localStorage.removeItem('basket_id');
  }

  calculateShippingPrice(shippingOption: ShippingOption) {
    this.shipping = shippingOption.price;
    const basket = this.gettingValueOfBasket();
    basket.shippingOptionId = shippingOption.id;
    basket.shippingPrice = shippingOption.price;
    this.calculateBasketSum();
    this.updateClientBasket(basket);
  }

  persistingPaymentOption(paymentOption: PaymentOption) {
    const basket = this.gettingValueOfBasket();
    basket.paymentOptionId = paymentOption.id;
    this.updateClientBasket(basket);
  }

  createPaymentIntent() {
    return this.http.post(this.baseUrl + 'payments/' + this.gettingValueOfBasket().id, {})
      .pipe(
        map((basket: Basket) => {
          this.basketBSubject.next(basket);
        })
      );
  }

  private mapItemToBasketItem(childrenItem: ChildrenItem, quantity: number): BasketChildrenItem {
    return {
      id: childrenItem.id,
      childrenItemName: childrenItem.name,
      price: childrenItem.price,
      picture: childrenItem.picture,
      stockQuantity: childrenItem.stockQuantity,
      discountedPrice: childrenItem.discountedPrice,
      quantity
    };
  }

  private addingOrUpdatingBasketItem(basketitems: BasketChildrenItem[], itemToAdd: BasketChildrenItem,
                                     quantity: number): BasketChildrenItem[] {
   const index = basketitems.findIndex(i => i.id === itemToAdd.id);
   if (index === -1) {
   itemToAdd.quantity = quantity;
   basketitems.push(itemToAdd);
   } else {
   basketitems[index].quantity += quantity;
   }
   return basketitems;
}

  private creatingBasket(): Basket {
    const basket = new BasketClass();
    localStorage.setItem('basket_id', basket.id);
    return basket;
  }

  private calculateBasketSum() {
    const basket = this.gettingValueOfBasket();
    const shipping = this.shipping;
    if (basket.basketChildrenItems.some(x => x.discountedPrice !== null)) {
      const discounttotal = basket.basketChildrenItems.filter(x => x.discountedPrice !== null);
      const regulartotal = basket.basketChildrenItems.filter(x => x.discountedPrice === null);
      const discounttotal1 = discounttotal.reduce((a, b) => (b.discountedPrice * b.quantity) + a, 0);
      const regulartotal1 = regulartotal.reduce((a, b) => (b.price * b.quantity) + a, 0);
      const subtotal = discounttotal1 + regulartotal1;
      const total = subtotal + shipping;
      this.basketSumBSubject.next({shipping, total, subtotal});
    } else {
      const subtotal = basket.basketChildrenItems.reduce((a, b) => (b.price * b.quantity) + a, 0);
      const total = subtotal + shipping;
      this.basketSumBSubject.next({shipping, total, subtotal});
    }
  }

}
