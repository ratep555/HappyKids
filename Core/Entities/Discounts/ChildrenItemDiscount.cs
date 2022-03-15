using Core.Entities.ChildrenItems;
using Core.Entities.Discounts;

namespace Core.Entities
{
    public class ChildrenItemDiscount
    {
        public int ChildrenItemId { get; set; }
        public ChildrenItem ChildrenItem { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}