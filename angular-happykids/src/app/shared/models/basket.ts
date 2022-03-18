import { v4 as uuidv4 } from 'uuid';

export interface Basket {
    id: string;
    basketChildrenItems: BasketChildrenItem[];
    clientSecret?: string;
    shippingOptionId?: number;
    paymentOptionId?: number;
    shippingPrice?: number;
    paymentIntentId?: string;
}

export interface BasketChildrenItem {
    id: number;
    childrenItemName: string;
    price: number;
    quantity: number;
    picture: string;
    stockQuantity?: number;
    discountedPrice?: number;
}

export class BasketClass implements Basket {
    id = uuidv4();
    basketChildrenItems: BasketChildrenItem[] = [];
}

export interface BasketSum {
    shipping: number;
    subtotal: number;
    total: number;
}

