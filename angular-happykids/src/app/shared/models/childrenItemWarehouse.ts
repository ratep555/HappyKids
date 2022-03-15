export interface ChildrenItemWarehouse {
    childrenItemId: number;
    warehouseId: number;
    stockQuantity: number;
    reservedQuantity?: number;
    childrenItem: string;
    warehouse: string;
    city: string;
}

export interface ChildrenItemWarehouseCreateEdit {
    childrenItemId: number;
    warehouseId: number;
    stockQuantity: number;
    reservedQuantity?: number;
}
