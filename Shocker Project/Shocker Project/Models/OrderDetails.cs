﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shocker_Project.Models
{
    public partial class OrderDetails
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string CouponId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ReturnReason { get; set; }

        public virtual Coupons Coupon { get; set; }
        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}