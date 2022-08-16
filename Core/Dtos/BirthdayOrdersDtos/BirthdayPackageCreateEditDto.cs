using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Dtos.BirthdayOrdersDtos
{
    /// <summary>
    /// <param name="BinderType = typeof">Since we are using [FromForm] in BirthdayPackagesController for creating/updating birthday package,</param>
    /// <param name="BinderType = typeof">we are helping modelbinder to bind data it will receive.</param>
    /// See BirthdayPckagesController/Create/UpdateBirthdayPackage for more details
    /// </summary>
    public class BirthdayPackageCreateEditDto
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string PackageName { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        public IFormFile Picture { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<int>))]
        public int NumberOfParticipants { get; set; }
        

        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal Price { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<decimal>))]
        public decimal AdditionalBillingPerParticipant { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<int>))]
        public int Duration { get; set; }

        
        [ModelBinder(BinderType = typeof(TypeBinder<bool?>))]
        public decimal? DiscountedPrice { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<bool?>))]
        public bool? HasDiscountsApplied { get; set; }

        
        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> KidActivitiesIds { get; set; }


        [ModelBinder(BinderType = typeof(TypeBinder<List<int>>))]
        public List<int> DiscountsIds { get; set; }
    }
}