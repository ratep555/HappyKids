import { Category } from './category';
import { ChildrenItem } from './childrenitem';
import { ChildrenItemWarehouse } from './childrenItemWarehouse';
import { Manufacturer } from './manufacturer';
import { Order } from './order';
import { PaymentOption } from './paymentOption';
import { ShippingOption } from './shippingOption';
import { Tag } from './tag';
import { Warehouse } from './warehouse';

export interface IPaginationForCategories {
    page: number;
    pageCount: number;
    count: number;
    data: Category[];
  }

export interface IPaginationForChildrenItemWarehouses {
    page: number;
    pageCount: number;
    count: number;
    data: ChildrenItemWarehouse[];
  }

export interface IPaginationForManufacturers {
    page: number;
    pageCount: number;
    count: number;
    data: Manufacturer[];
  }
export interface IPaginationForChildrenItemsOrders {
    page: number;
    pageCount: number;
    count: number;
    data: Order[];
  }

export interface IPaginationForPaymentOptions {
    page: number;
    pageCount: number;
    count: number;
    data: PaymentOption[];
  }

export interface IPaginationForShippingOptions {
    page: number;
    pageCount: number;
    count: number;
    data: ShippingOption[];
  }

export interface IPaginationForTags {
    page: number;
    pageCount: number;
    count: number;
    data: Tag[];
  }

export interface IPaginationForWarehouses {
    page: number;
    pageCount: number;
    count: number;
    data: Warehouse[];
  }

export interface IPaginationForChildrenItems {
    page: number;
    pageCount: number;
    count: number;
    data: ChildrenItem[];
  }

export class PaginationForChildrenItems implements IPaginationForChildrenItems {
    page: number;
    pageCount: number;
    count: number;
    data: ChildrenItem[];
  }
