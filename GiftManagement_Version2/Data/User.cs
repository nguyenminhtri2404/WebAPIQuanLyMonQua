using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GiftManagement_Version2.Data
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Image { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int Point { get; set; }
        public int AccumulatedPoint { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Ranking> Rankings { get; set; }
    }
}
