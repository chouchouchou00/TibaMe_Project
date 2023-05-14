﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Shocker_Project.Areas.Models
{
    public partial class Users
    {
        public Users()
        {
            Addresses = new HashSet<Addresses>();
            ClientCasesAdminAccountNavigation = new HashSet<ClientCases>();
            ClientCasesUserAccountNavigation = new HashSet<ClientCases>();
            CouponsHolderAccountNavigation = new HashSet<Coupons>();
            CouponsPublisherAccountNavigation = new HashSet<Coupons>();
            OrdersBuyerAccountNavigation = new HashSet<Orders>();
            OrdersSellerAccountNavigation = new HashSet<Orders>();
            Products = new HashSet<Products>();
        }

        public string Account { get; set; }
        public string AccountType { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset? BirthDate { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public DateTimeOffset RegisterDate { get; set; }

        public virtual ICollection<Addresses> Addresses { get; set; }
        public virtual ICollection<ClientCases> ClientCasesAdminAccountNavigation { get; set; }
        public virtual ICollection<ClientCases> ClientCasesUserAccountNavigation { get; set; }
        public virtual ICollection<Coupons> CouponsHolderAccountNavigation { get; set; }
        public virtual ICollection<Coupons> CouponsPublisherAccountNavigation { get; set; }
        public virtual ICollection<Orders> OrdersBuyerAccountNavigation { get; set; }
        public virtual ICollection<Orders> OrdersSellerAccountNavigation { get; set; }
        public virtual ICollection<Products> Products { get; set; }
    }
}