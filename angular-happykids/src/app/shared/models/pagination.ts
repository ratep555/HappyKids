import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { ClientBirthdayOrder } from './birthdayorder';
import { BirthdayPackage } from './birthdaypackage';
import { Blog } from './blog';
import { Branch } from './branch';
import { Category } from './category';
import { ChildrenItem } from './childrenitem';
import { ChildrenItemWarehouse } from './childrenItemWarehouse';
import { Discount } from './discount';
import { KidActivity } from './kidactivity';
import { Manufacturer } from './manufacturer';
import { ClientMessage } from './message';
import { Order } from './order';
import { OrderStatus } from './orderStatus';
import { PaymentOption } from './paymentOption';
import { ShippingOption } from './shippingOption';
import { Tag } from './tag';
import { User } from './user';
import { Warehouse } from './warehouse';

export interface IPaginationForBranches {
    page: number;
    pageCount: number;
    count: number;
    data: Branch[];
  }

export interface IPaginationForCategories {
    page: number;
    pageCount: number;
    count: number;
    data: Category[];
  }

export interface IPaginationForClientBirthayOrders {
    page: number;
    pageCount: number;
    count: number;
    data: ClientBirthdayOrder[];
  }

export interface IPaginationForDiscounts {
    page: number;
    pageCount: number;
    count: number;
    data: Discount[];
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

export interface IPaginationForMessages {
    page: number;
    pageCount: number;
    count: number;
    data: ClientMessage[];
}

export interface IPaginationForChildrenItemsOrders {
    page: number;
    pageCount: number;
    count: number;
    data: Order[];
  }

export interface IPaginationForOrderStatuses {
    page: number;
    pageCount: number;
    count: number;
    data: OrderStatus[];
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

export interface IPaginationForUsers {
    page: number;
    pageCount: number;
    count: number;
    data: User[];
  }

export interface IPaginationForWarehouses {
    page: number;
    pageCount: number;
    count: number;
    data: Warehouse[];
  }

export interface IPaginationForBirthdayPackages {
    page: number;
    pageCount: number;
    count: number;
    data: BirthdayPackage[];
  }

export class PaginationForBirthdayPackages implements IPaginationForBirthdayPackages{
    page: number;
    pageCount: number;
    count: number;
    data: BirthdayPackage[];
  }

export interface IPaginationForBlogs {
    page: number;
    pageCount: number;
    count: number;
    data: Blog[];
  }

export class PaginationForBlogs implements IPaginationForBlogs {
    page: number;
    pageCount: number;
    count: number;
    data: Blog[];
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

export interface IPaginationForKidActivities {
    page: number;
    pageCount: number;
    count: number;
    data: KidActivity[];
  }

export class PaginationForKidActivities implements IPaginationForKidActivities {
    page: number;
    pageCount: number;
    count: number;
    data: KidActivity[];
  }
