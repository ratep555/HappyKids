using System.Collections.Generic;

namespace Core.Dtos.BasketsDtos
{
    public class ClientBasketDto
    {
        public string Id { get; set; }
        public List<BasketChildrenItemDto> BasketChildrenItems { get; set; }
        public int? ShippingOptionId { get; set; }
        public int? PaymentOptionId { get; set; }
        public string ClientSecret { get; set; }
        public string PaymentIntentId { get; set; }
        public decimal ShippingPrice { get; set; }
    }
}