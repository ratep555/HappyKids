namespace Core.Dtos.OrdersDtos
{
    public class ShippingAddressDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int CountryId { get; set; }
    }
}