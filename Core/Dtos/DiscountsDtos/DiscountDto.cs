using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Dtos.ChildrenItemsDtos;

namespace Core.Dtos.DiscountsDto
{
    public class DiscountDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DiscountPercentage { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public List<ChildrenItemDto> ChildrenItems { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<ManufacturerDto> Manufacturers { get; set; }
    }
}





