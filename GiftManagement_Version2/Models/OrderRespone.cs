namespace GiftManagement_Version2.Models
{
    public class OrderRespone
    {
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public int GiftId { get; set; }
        public int Quantity { get; set; }
        public int? MainGiftId { get; set; }
        public DateTime OrderAt { get; set; }
        public int OrderType { get; set; }
    }
}
