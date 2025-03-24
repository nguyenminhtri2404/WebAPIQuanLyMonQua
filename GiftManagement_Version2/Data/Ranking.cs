using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftManagement_Version2.Data
{
    public class Ranking
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int TotalPoint { get; set; }
        public DateTime LastOrderDate { get; set; }
        public int Rank { get; set; }
        public int IsFinalized { get; set; } //Trạng thái đã xác nhận hay chưa
    }
}
