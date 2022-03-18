using System;
using System.Collections.Generic;

namespace Core.Dtos.OrdersDtos
{
    public class ClientOrderToReturnDto
    {
        public int Id { get; set; }
        public string CustomerEmail { get; set; }
        public DateTimeOffset DateOfCreation { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public string ShippingOption { get; set; }
        public string PaymentOption { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal Subtotal { get; set; }
        public string OrderStatus { get; set; }
        public int? OrderStatusId { get; set; }

        public List<OrderChildrenItemDto> OrderChildrenItems { get; set; }
    }
}