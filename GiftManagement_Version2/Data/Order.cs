using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftManagement_Version2.Data
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
        public int Quantity { get; set; }
        public int? MainGiftId { get; set; }
        public Order MainGift { get; set; }
        public DateTime OrderAt { get; set; }
        public int OrderType { get; set; } //0: Recived, 1: Buy
        public int TotalPrice { get; set; }
    }
}
