namespace Core.Entities.Discounts
{
    public class ManufacturerDiscount
    {
        public int ManufacturerId { get; set; }
        public Manufacturer Manufacturer { get; set; }

        public int DiscountId { get; set; }
        public Discount Discount { get; set; }
    }
}