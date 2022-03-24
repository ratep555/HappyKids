using System.Collections.Generic;
using Core.Dtos.BirthdayOrdersDtos;
using Core.Dtos.ChildrenItemsDtos;
using Core.Dtos.DiscountsDto;

namespace Core.Dtos.DiscountsDtos
{
    public class DiscountPutGetDto
    {
        public DiscountDto Discount { get; set; }
        public IEnumerable<BirthdayPackageDto> SelectedBirthdayPackages { get; set; }
        public IEnumerable<BirthdayPackageDto> NonSelectedBirthdayPackages { get; set; }
        public IEnumerable<ChildrenItemDto> SelectedChildrenItems { get; set; }
        public IEnumerable<ChildrenItemDto> NonSelectedChildrenItems { get; set; }
        public IEnumerable<CategoryDto> SelectedCategories { get; set; }
        public IEnumerable<CategoryDto> NonSelectedCategories { get; set; }
        public IEnumerable<ManufacturerDto> NonSelectedManufacturers { get; set; }
        public IEnumerable<ManufacturerDto> SelectedManufacturers { get; set; }
    }
}


