using Core.Entities.ClientBaskets;

namespace Core.Entities.Orders
{
    public class OrderChildrenItem : BaseEntity
    {
        public OrderChildrenItem()
        {
            
        }

        public OrderChildrenItem(BasketChildrenItemOrdered basketItemOrdered, decimal price, int quantity)
        {
            BasketChildrenItemOrdered = basketItemOrdered;
            Price = price;
            Quantity = quantity;
        }

        public BasketChildrenItemOrdered BasketChildrenItemOrdered { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}