import { Category } from './category';
import { Discount } from './discount';
import { Manufacturer } from './manufacturer';
import { Tag } from './tag';

export interface ChildrenItem {
    id: number;
    name: string;
    description: string;
    price: number;
    picture: string;
    averageVote: number;
    userVote: number;
    count: number;
    stockQuantity?: number;
    discountedPrice?: number;
    hasDiscountsApplied?: boolean;
    likesCount?: number;
    discountSum: number;
    discounts: Discount[];
}

export interface ChildrenItemCreateEdit {
    id: number;
    name: string;
    description: string;
    price: number;
    picture: File;
    categoriesIds: number[];
    manufacturersIds: number[];
    tagsIds: number[];
    discountsIds: number[];
}

export interface ChildrenItemPutGet {
    childrenItem: ChildrenItem;
    selectedCategories: Category[];
    nonSelectedCategories: Category[];
    selectedDiscounts: Discount[];
    nonSelectedDiscounts: Discount[];
    selectedManufacturers: Manufacturer[];
    nonSelectedManufacturers: Manufacturer[];
    selectedTags: Tag[];
    nonSelectedTags: Tag[];
}
