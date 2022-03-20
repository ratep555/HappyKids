using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core.Entities.Orders;

namespace Core.Entities.BirthdayOrders
{
    public class ClientBirthdayOrder : BaseEntity
    {
         public int BranchId { get; set; }
        public Branch Branch { get; set; }

        public int BirthdayPackageId { get; set; }
        public BirthdayPackage BirthdayPackage { get; set; }

        public string ClientName { get; set; }
        public string BirthdayGirlBoyName { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public int NumberOfGuests { get; set; }
        public int BirthdayNo { get; set; }
        public decimal Price { get; set; }
        public string Remarks { get; set; }
       

        [DataType(DataType.Date)]
        public DateTime StartDateAndTime { get; set; }
        

        [DataType(DataType.Date)]
        public DateTime EndDateAndTime { get; set; }

        public int? OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}