namespace Shocker_Project.Models
{
    public class PrViewModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int UnitPrice { get; set; }
        public string PictureId { get; set; }
        public IFormFile Picture { get; set; }
    }
}
