using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Discounts;

namespace Core.Entities.BirthdayOrders
{
    public class BirthdayPackage : BaseEntity
    {
        [Required]
		[MaxLength(255)]
        public string PackageName { get; set; }

        [Required]
		[MaxLength(2000)]
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public decimal Price { get; set; }
        public decimal AdditionalBillingPerParticipant { get; set; }
        public int Duration { get; set; }
        public string Picture { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public bool? HasDiscountsApplied { get; set; }

        public ICollection<BirthdayPackageKidActivity> BirthdayPackageKidActivities { get; set; }
        public ICollection<BirthdayPackageDiscount> BirthdayPackageDiscounts { get; set; }
        public ICollection<ClientBirthdayOrder> ClientBirthdayOrders { get; set; }
    }
}