using System.ComponentModel.DataAnnotations;
using Core.Entities.ChildrenItems;
using Core.Entities.Identity;

namespace Core.Entities
{
     public class Rating : BaseEntity
    {
        [Range(1, 5)]
        public int Rate { get; set; }

        public int ApplicationUserId { get; set; }
        public ApplicationUser Client { get; set; }

        public int ChildrenItemId { get; set; }
        public ChildrenItem ChildrenItem { get; set; }
    }
}