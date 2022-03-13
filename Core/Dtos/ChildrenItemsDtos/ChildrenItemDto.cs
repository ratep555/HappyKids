using System.Collections.Generic;
using Core.Dtos.DiscountsDto;

namespace Core.Dtos.ChildrenItemsDtos
{
    public class ChildrenItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Picture { get; set; }
        public int? StockQuantity { get; set; }
        public int UserVote { get; set; }
        public int? RateSum { get; set; }
        public int? Count { get; set; }
        public double? AverageVote { get; set; }
        public bool? NotReturnable { get; set; }
        public bool? HasDiscountsApplied { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public decimal DiscountSum { get; set; }
        public int? LikesCount { get; set; }

        public List<CategoryDto> Categories { get; set; }
        public List<DiscountDto> Discounts { get; set; }    
        public List<ManufacturerDto> Manufacturers { get; set; }        
        public List<TagDto> Tags { get; set; }    
        public List<WarehouseDto> Warehouses { get; set; } 
    }
}