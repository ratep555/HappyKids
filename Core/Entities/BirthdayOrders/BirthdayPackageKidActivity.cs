namespace Core.Entities.BirthdayOrders
{
    public class BirthdayPackageKidActivity
    {
        public int BirthdayPackageId { get; set; }
        public BirthdayPackage BirthdayPackage { get; set; }

        public int KidActivityId { get; set; }
        public KidActivity KidActivity { get; set; }
    }
}