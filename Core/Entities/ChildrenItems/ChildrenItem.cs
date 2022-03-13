using System.Collections.Generic;

namespace Core.Entities.ChildrenItems
{
    public class ChildrenItem : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public string Picture { get; set; }
        public int? StockQuantity { get; set; }
        public bool? NotReturnable { get; set; }
        public bool? HasDiscountsApplied { get; set; }

        public ICollection<ChildrenItemCategory> ChildrenItemCategories { get; set; }
        public ICollection<ChildrenItemTag> ChildrenItemTags { get; set; }
        public ICollection<ChildrenItemManufacturer> ChildrenItemManufacturers { get; set; }
        public ICollection<ChildrenItemDiscount> ChildrenItemDiscounts { get; set; }
        public ICollection<ChildrenItemWarehouse> ChildrenItemWarehouses { get; set; }
    }
}