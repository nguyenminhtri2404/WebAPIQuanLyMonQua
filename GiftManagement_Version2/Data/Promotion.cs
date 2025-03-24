using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftManagement_Version2.Data
{
    public class Promotion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int MainGiftId { get; set; }
        public Gift MainGift { get; set; }
        public int BonusGiftId { get; set; }
        public Gift BonusGift { get; set; }
    }
}
