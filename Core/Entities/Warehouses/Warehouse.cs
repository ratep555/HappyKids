namespace Core.Entities
{
    public class Warehouse : BaseEntity
    {
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Name { get; set; }

    }
}