using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftManagement_Version2.Data
{
    public class Gift
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int GiftType { get; set; } //0: MainGift, 1: BonusGift
        public string? Image { get; set; }
        public int Quantity { get; set; }
        public int Point { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Promotion> Promotions { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Cart> Carts { get; set; }
    }

}
