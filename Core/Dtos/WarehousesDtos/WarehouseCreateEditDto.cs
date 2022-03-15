namespace Core.Dtos.WarehousesDtos
{
    public class WarehouseCreateEditDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
    }
}