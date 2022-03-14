using System;
using System.Collections.Generic;

namespace Core.Entities.Orders
{
    public class ClientOrder : BaseEntity
    {
        public ClientOrder()
        {
            
        }

        public ClientOrder(ICollection<OrderChildrenItem> orderChildrenItems,
            string customerEmail,
            ShippingAddress shippingAddress,
            ShippingOption shippingOption,
            PaymentOption paymentOption,
            decimal subtotal,
            string paymentIntentId)
        {
            OrderChildrenItems = orderChildrenItems;
            CustomerEmail = customerEmail;
            ShippingAddress = shippingAddress;
            ShippingOption = shippingOption;
            PaymentOption = paymentOption;
            Subtotal = subtotal;
            PaymentIntentId = paymentIntentId;
        }
        
        public string CustomerEmail { get; set; }
        public ShippingAddress ShippingAddress { get; set; }
        public DateTimeOffset DateOfCreation { get; set; } = DateTimeOffset.Now;
        public decimal Subtotal { get; set; }
        
        public int ShippingOptionId { get; set; }
        public ShippingOption ShippingOption { get; set; }

        public int PaymentOptionId { get; set; }
        public PaymentOption PaymentOption { get; set; }

        public int? OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public string PaymentIntentId { get; set; }
        public ICollection<OrderChildrenItem> OrderChildrenItems { get; set; }

        public decimal GetTotal()
        {
            return Subtotal + ShippingOption.Price;
        }
    }
}

