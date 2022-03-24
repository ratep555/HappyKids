using System.Collections.Generic;
using Core.Dtos.DiscountsDto;

namespace Core.Dtos.BirthdayOrdersDtos
{
    public class BirthdayPackageDto
    {
        public int Id { get; set; }
        public string PackageName { get; set; }
        public string Description { get; set; }
        public int NumberOfParticipants { get; set; }
        public decimal Price { get; set; }
        public decimal AdditionalBillingPerParticipant { get; set; }
        public decimal DiscountedAdditionalBillingPerParticipant { get; set; }
        public int Duration { get; set; }
        public string Picture { get; set; }
        public decimal DiscountSum { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public bool? HasDiscountsApplied { get; set; }

        public ICollection<KidActivityDto> KidActivities { get; set; }
        public ICollection<DiscountDto> Discounts { get; set; }
        public ICollection<ClientBirthdayOrderDto> Birthdays { get; set; }
    }
}