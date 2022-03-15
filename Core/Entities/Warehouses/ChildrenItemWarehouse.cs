namespace Core.Entities.ChildrenItems
{
    public class ChildrenItemWarehouse
    {
        public int ChildrenItemId { get; set; }
        public ChildrenItem ChildrenItem { get; set; }

        public int WarehouseId { get; set; }
        public Warehouse Warehouse { get; set; }

        public int StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
    }
}