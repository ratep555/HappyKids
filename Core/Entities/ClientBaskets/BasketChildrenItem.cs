namespace Core.Entities.ClientBaskets
{
    public class BasketChildrenItem
    {
        public int Id { get; set; }
        public string ChildrenItemName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Picture { get; set; }
        public int? StockQuantity { get; set; }
        public decimal? DiscountedPrice { get; set; }
    }
}