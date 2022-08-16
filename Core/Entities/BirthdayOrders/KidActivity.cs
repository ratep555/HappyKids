using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities.BirthdayOrders
{
    public class KidActivity : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        public string Picture { get; set; }
        public string VideoClip { get; set; }

        public ICollection<BirthdayPackageKidActivity> BirthdayPackageKidActivities { get; set; }
    }
}