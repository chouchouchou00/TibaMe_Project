using Shocker_Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Shocker_Project.Areas.Admin.Models
{
    public class CouponsViewModels
    {
        //public Coupons()
        //{
        //    OrderDetails = new HashSet<OrderDetails>();
        //}

        public string CouponId { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string HolderAccount { get; set; }
        public int ProductCategoryId { get; set; }
        public  string CategoryName { get; set; }
        //[Required(ErrorMessage="請輸入折扣小於1")]
        public decimal Discount { get; set; }
        public string Status { get; set; }
        public string StatusName { get; set; }
        public string PublisherAccount { get; set; }

        
    }
}
