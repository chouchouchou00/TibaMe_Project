﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Shocker_Project.Areas.Models
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
            Pictures = new HashSet<Pictures>();
            Ratings = new HashSet<Ratings>();
        }

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

        public virtual ProductCategories ProductCategory { get; set; }
        public virtual Users SellerAccountNavigation { get; set; }
        public virtual ICollection<OrderDetails> OrderDetails { get; set; }
        public virtual ICollection<Pictures> Pictures { get; set; }
        public virtual ICollection<Ratings> Ratings { get; set; }
    }
}