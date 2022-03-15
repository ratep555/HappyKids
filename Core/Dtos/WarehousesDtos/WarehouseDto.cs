namespace Core.Dtos
{
    public class WarehouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
        public string Country { get; set; }
    }
}