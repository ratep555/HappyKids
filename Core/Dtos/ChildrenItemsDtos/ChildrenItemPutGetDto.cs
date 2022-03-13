using System.Collections.Generic;
using Core.Dtos.DiscountsDto;

namespace Core.Dtos.ChildrenItemsDtos
{
    public class ChildrenItemPutGetDto
    {
        public ChildrenItemDto ChildrenItem { get; set; }
        public IEnumerable<CategoryDto> SelectedCategories { get; set; }
        public IEnumerable<CategoryDto> NonSelectedCategories { get; set; }
        public IEnumerable<DiscountDto> SelectedDiscounts { get; set; }
        public IEnumerable<DiscountDto> NonSelectedDiscounts { get; set; }
        public IEnumerable<ManufacturerDto> SelectedManufacturers { get; set; }
        public IEnumerable<ManufacturerDto> NonSelectedManufacturers { get; set; }
        public IEnumerable<TagDto> SelectedTags { get; set; }
        public IEnumerable<TagDto> NonSelectedTags { get; set; }
    }
}