using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.ChildrenItems
{
    public class ChildrenItem : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
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