using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftManagement_Version2.Data
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int GiftId { get; set; }
        public Gift Gift { get; set; }
        public int Quantity { get; set; }
        public int Status { get; set; } //0: InCart, 1: Ordered 
    }
}
