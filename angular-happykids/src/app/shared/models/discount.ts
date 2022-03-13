import { Category } from './category';
import { ChildrenItem } from './childrenitem';
import { Manufacturer } from './manufacturer';

export interface Discount {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    minimumOrderValue?: number;
    childrenitems: ChildrenItem[];
}

export interface DiscountCreateEdit {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    minimumOrderValue?: number;
    childrenitems: ChildrenItem[];
    categories: Category[];
    manufacturers: Manufacturer[];
}

export class DiscountEditClass {
    id: number;
    name: string;
    discountPercentage: number;
    startDate: Date;
    endDate: Date;
    minimumOrderValue?: number;
    childrenitems: ChildrenItem[];
    categories: Category[];
    manufacturers: Manufacturer[];
}

export interface DiscountPutGet {
    discount: Discount;
    selectedChildrenitems: ChildrenItem[];
    nonSelectedChildrenitems: ChildrenItem[];
    selectedCategories: Category[];
    nonSelectedCategories: Category[];
    nonSelectedManufacturers: Manufacturer[];
    selectedManufacturers: Manufacturer[];
}







