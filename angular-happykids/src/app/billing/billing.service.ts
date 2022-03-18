import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ClientOrder } from '../shared/models/order';
import { PaymentOption } from '../shared/models/paymentOption';
import { ShippingOption } from '../shared/models/shippingOption';

@Injectable({
  providedIn: 'root'
})
export class BillingService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  createOrder(order: ClientOrder) {
    return this.http.post(this.baseUrl + 'orders', order);
  }

  createOrderForStripe(order: ClientOrder) {
    return this.http.post(this.baseUrl + 'orders/stripe', order);
  }

  getShippingOptions() {
    return this.http.get<ShippingOption[]>(this.baseUrl + 'orders/shippingoptions');
  }

  getPaymentOptions() {
    return this.http.get<PaymentOption[]>(this.baseUrl + 'orders/paymentoptions');
  }

  getStripePaymentOption() {
    return this.http.get<PaymentOption>(this.baseUrl + 'orders/stripepay');
  }
}
