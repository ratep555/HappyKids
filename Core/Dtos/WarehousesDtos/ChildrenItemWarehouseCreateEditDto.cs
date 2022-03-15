namespace Core.Dtos.WarehousesDtos
{
    public class ChildrenItemWarehouseCreateEditDto
    {
        public int ChildrenItemId { get; set; }
        public int WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
    }
}