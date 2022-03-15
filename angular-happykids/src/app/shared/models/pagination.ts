import { ChildrenItem } from './childrenitem';
import { ChildrenItemWarehouse } from './childrenItemWarehouse';
import { Tag } from './tag';
import { Warehouse } from './warehouse';

export interface IPaginationForChildrenItemWarehouses {
    page: number;
    pageCount: number;
    count: number;
    data: ChildrenItemWarehouse[];
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
