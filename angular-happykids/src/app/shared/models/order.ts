import { Address } from './address';

export interface ClientOrder {
    basketId: string;
    shippingOptionId: number;
    paymentOptionId: number;
    shippingAddress: Address;
}

export interface OrderChildrenItem {
    childrenItemId: number;
    childrenItemName: string;
    picture: string;
    price: number;
    quantity: number;
}

export interface Order {
    id: number;
    customerEmail: string;
    dateOfCreation: Date;
    shippingAddress: Address;
    shippingOption: string;
    paymentOption: string;
    shippingPrice: number;
    orderChildrenItems: OrderChildrenItem[];
    subtotal: number;
    getTotal: number;
    orderStatusId?: number;
    orderStatus: string;
    country: string;
}
