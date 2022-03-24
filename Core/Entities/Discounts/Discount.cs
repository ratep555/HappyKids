using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.Discounts
{
    public class Discount : BaseEntity
    {
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }


        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }


        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public ICollection<BirthdayPackageDiscount> BirthdayPackageDiscounts { get; set; }
        public ICollection<ChildrenItemDiscount> ChildrenItemDiscounts { get; set; }
        public ICollection<CategoryDiscount> CategoryDiscounts { get; set; }
        public ICollection<ManufacturerDiscount> ManufacturerDiscounts { get; set; }


    }
}