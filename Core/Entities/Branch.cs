using NetTopologySuite.Geometries;

namespace Core.Entities
{
    public class Branch : BaseEntity
    {
        public int CountryId { get; set; }
        public Country Country { get; set; }

        public string City { get; set; }
        public string Street { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string WorkingHours { get; set; }
        public Point Location { get; set; }
    }
}