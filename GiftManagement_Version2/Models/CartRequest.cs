namespace GiftManagement_Version2.Models
{
    public class CartRequest
    {
        public int UserId { get; set; }
        public int GiftId { get; set; }
        public int Quantity { get; set; }
    }
}
