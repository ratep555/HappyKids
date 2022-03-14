namespace Core.Dtos.OrdersDtos
{
    public class ClientOrderDto
    {
        public string BasketId { get; set; }
        public int ShippingOptionId { get; set; }
        public int PaymentOptionId { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
    }
}