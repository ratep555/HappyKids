using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Dtos.ChildrenItemsDtos
{
    /// <summary>
    /// <param name="BinderType = typeof">Since we are using [FromForm] in ChildrenItemsController for creating/updating children item,</param>
    /// <param name="BinderType = typeof">we are helping modelbinder to bind data it will receive.</param>
    /// See ChildrenItemsController/Create/UpdateChildrenItem for more details
    /// </summary>
    public class ChildrenItemCreateEditDto
    {
        public int Id { get; set; }

        [Required, MaxLength(255)]
        public string Name { get; set; }

        [Required, MaxLength(2000)]
        public string Description { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal Price { get; set; }

        public IFormFile Picture { get; set; }
        

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> CategoriesIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> DiscountsIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> ManufacturersIds { get; set; }
        

        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> TagsIds { get; set; }
    }
}

