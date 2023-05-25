using Shocker_Project.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Shocker_Project.Areas.Shoppingcart.Views.ViewModel
{
    public class ShoppingCartOrderDetailViewModel
    {
        public int OrderId { get; set; }
        [DisplayName("產品編號")]
        public int ProductId { get; set; }
        [DisplayName("優惠券")]
        public string CouponId { get; set; }
        [DisplayName("數量")]
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string ReturnReason { get; set; }

        public virtual Coupons Coupon { get; set; }
        public virtual Orders Order { get; set; }
        public virtual Products Product { get; set; }
    }
}

