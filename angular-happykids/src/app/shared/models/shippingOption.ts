export interface ShippingOption {
    id: number;
    name: string;
    description: string;
    transitDays: string;
    price: number;
}

export interface ShippingOptionCreateEdit {
    id: number;
    name: string;
    description: string;
    transitDays: string;
    price: number;
}
