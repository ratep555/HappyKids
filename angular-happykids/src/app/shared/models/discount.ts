import { Category } from './category';
import { ChildrenItem } from './childrenitem';
import { Manufacturer } from './manufacturer';

export interface Discount {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    childrenitems: ChildrenItem[];
}

export interface DiscountCreateEdit {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    childrenItems: ChildrenItem[];
    categories: Category[];
    manufacturers: Manufacturer[];
}

export class DiscountEditClass {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    childrenitems: ChildrenItem[];
    categories: Category[];
    manufacturers: Manufacturer[];
}

export interface DiscountPutGet {
    discount: Discount;
    selectedChildrenItems: ChildrenItem[];
    nonSelectedChildrenItems: ChildrenItem[];
    selectedCategories: Category[];
    nonSelectedCategories: Category[];
    nonSelectedManufacturers: Manufacturer[];
    selectedManufacturers: Manufacturer[];
}







