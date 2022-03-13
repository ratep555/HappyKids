import { ChildrenItem } from './childrenitem';

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
