using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Shocker_Project.Models.ViewModel
{
    public class ShoppingCartOrderViewModel
    {
        public int OrderId { get; set; }
        public string? BuyerAccount { get; set; }
        [DisplayName("地址")]
        public string? Address { get; set; }
        public DateTimeOffset OrderDate { get; set; }
        public DateTimeOffset RequiredDate { get; set; }
        [DisplayName("行動電話")]
        [RegularExpression("@{0-9}[10]", ErrorMessage = "電話號碼必須10個數字")]
        public string BuyerPhone { get; set; }
        public string? PayMethod { get; set; }
    }
}
