using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace Core.Dtos.DiscountsDtos
{
     /// <summary>
    /// <param name="BinderType = typeof">We are helping modelbinder to bind data it will receive.</param>
    /// </summary>
    public class DiscountCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal DiscountPercentage { get; set; }

        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ChildrenItemsIds { get; set; } 
        
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CategoriesIds { get; set; }    
        
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ManufacturersIds { get; set; }    
    }
}
