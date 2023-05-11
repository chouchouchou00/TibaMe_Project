﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Shocker.Models
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
            Ratings = new HashSet<Ratings>();
        }

        public int OrderId { get; set; }
        public string SellerAccount { get; set; }
        public string BuyerAccount { get; set; }
        public string Address { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset RequiredDate { get; set; }
        public int BuyerPhone { get; set; }
        public string PayMethod { get; set; }
        public string Status { get; set; }

        public virtual Addresses Addresses { get; set; }
        public virtual Users BuyerAccountNavigation { get; set; }
        public virtual Users SellerAccountNavigation { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<Ratings> Ratings { get; set; }
    }
}