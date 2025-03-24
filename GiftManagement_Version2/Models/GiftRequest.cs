namespace GiftManagement_Version2.Models
{
    public class GiftRequest
    {
        public string? Name { get; set; }
        public int GiftType { get; set; }
        public IFormFile? Image { get; set; }
        public int Quantity { get; set; }
        public int Point { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
