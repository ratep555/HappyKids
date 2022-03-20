using System.Collections.Generic;

namespace Core.Entities.BirthdayOrders
{
    public class KidActivity : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Picture { get; set; }
        public string VideoClip { get; set; }

        public ICollection<BirthdayPackageKidActivity> BirthdayPackageKidActivities { get; set; }
    }
}