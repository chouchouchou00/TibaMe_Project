﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Shocker_Project.Areas.Seller.Models
{
    public partial class Products
    {
        public int ProductId { get; set; }
        public string SellerAccount { get; set; }
        public DateTimeOffset LaunchDate { get; set; }
        public string ProductName { get; set; }
        public int ProductCategoryId { get; set; }
        public string Description { get; set; }
        public string UnitsInStock { get; set; }
        public string Sales { get; set; }
        public string UnitPrice { get; set; }
        public string Status { get; set; }
        public string Currency { get; set; }
    }
}