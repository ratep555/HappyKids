namespace Core.Dtos.WarehousesDtos
{
    public class ChildrenItemWarehouseDto
    {
        public int ChildrenItemId { get; set; }
        public int WarehouseId { get; set; }
        public int StockQuantity { get; set; }
        public int? ReservedQuantity { get; set; }
        public string ChildrenItem { get; set; }
        public string Warehouse { get; set; }
        public string City { get; set; }
    }
}