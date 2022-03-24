using System.Collections.Generic;
using Core.Dtos.DiscountsDto;

namespace Core.Dtos.BirthdayOrdersDtos
{
    public class BirthdayPackagePutGetDto
    {
        public BirthdayPackageDto BirthdayPackage { get; set; }
        public IEnumerable<KidActivityDto> SelectedKidActivities { get; set; }
        public IEnumerable<KidActivityDto> NonSelectedKidActivities { get; set; }
        public IEnumerable<DiscountDto> SelectedDiscounts { get; set; }
        public IEnumerable<DiscountDto> NonSelectedDiscounts { get; set; }
    }
}