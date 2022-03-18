namespace Core.Dtos.OrdersDtos
{
    public class OrderChildrenItemDto
    {
        public int ChildrenItemId { get; set; }
        public string ChildrenItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}