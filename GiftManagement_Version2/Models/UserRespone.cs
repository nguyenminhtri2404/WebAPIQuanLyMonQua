namespace GiftManagement_Version2.Models
{
    public class UserRespone
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Image { get; set; }
        public List<string> Permissions { get; set; }
    }
}
