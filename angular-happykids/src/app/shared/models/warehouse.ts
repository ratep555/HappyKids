export interface Warehouse {
    id: number;
    name: string;
    street: string;
    city: string;
    countryId: number;
    country: string;
}

export interface WarehouseCreateEdit {
    id: number;
    name: string;
    street: string;
    city: string;
    countryId: number;
}
